using DinoFracture;
using Game.Environment.EntitySystem;
using StarSmithGames.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Environment.DestructableSystem
{
	public abstract class FracturableMonoObject : MonoObject
	{
		public PreFracturedGeometry FractureGeometry;
		public Rigidbody Rigidbody;
		public Transform FractureRoot;
		[ Space ]
		public float Radius = 100f;
		public float Force = 30f;
		public float ForceThreshold;
		public LayerMask CollidableLayers = (LayerMask)int.MaxValue;

		private bool _isFractured = false;
		private List< GameObject > _fractures = new();
		private Vector3 _impactPoint;
		private Vector3 _impactImpulse;

		private void Awake()
		{
			foreach ( Transform child in FractureRoot )
			{
				_fractures.Add( child.gameObject );
				child.gameObject.SetActive( false );
			}

			FractureGeometry.OnFractureCompleted.AddListener( OnFractureCompleted );
		}

		public override void Dispose()
		{
			FractureGeometry.OnFractureCompleted.RemoveListener( OnFractureCompleted );
			
			base.Dispose();
		}

		public override void Destruct()
		{
			if ( _isFractured || FractureGeometry.IsProcessingFracture )
			{
				return;
			}
			_isFractured = true;

			FractureGeometry.GeneratedPieces = _fractures.RandomItem();
			
			Debug.LogError( _impactPoint );
			
			Vector3 localPoint = FractureGeometry.transform.worldToLocalMatrix.MultiplyPoint( _impactPoint );//new( Random.Range( -0.5f, 0.5f ), Random.Range( -0.5f, 0.5f ), Random.Range( -0.5f, 0.5f ));
			
			Debug.LogError( localPoint );
			
			FractureGeometry.FractureType = FractureType.Shatter;
			FractureGeometry.Fracture( localPoint ).SetCallbackObject(this);;
		}

		private void OnCollisionEnter( Collision col )
		{
			if (!FractureGeometry.IsProcessingFracture && col.contactCount > 0)
			{
				if ((CollidableLayers.value & (1 << col.gameObject.layer)) != 0)
				{
					_impactPoint = Vector3.zero;

					float sumSeparation = 0.0f;
					Vector3 avgNormal = Vector3.zero;
					for (int i = 0; i < col.contactCount; i++)
					{
						var contact = col.GetContact(i);
						if (contact.thisCollider.gameObject == gameObject)
						{
							float separation = Mathf.Max(1e-3f, contact.separation);

							_impactPoint += contact.point * separation;
							avgNormal -= contact.normal * separation;
							sumSeparation += separation;
						}
					}
					_impactPoint *= 1.0f / sumSeparation;
					avgNormal = avgNormal.normalized;

					_impactImpulse = -avgNormal * col.impulse.magnitude;

					float forceMag = 0.5f * _impactImpulse.sqrMagnitude;
					if (forceMag >= ForceThreshold)
					{
						Destruct();
					}
				}
			}
		}

		private void OnFracture( OnFractureEventArgs args )
		{
			if ( !args.IsValid ) return;
			
			Explode(args.FracturePiecesRootObject, args.OriginalMeshBounds, args.OriginalObject.transform.localScale);
		}

		private void OnFractureCompleted( OnFractureEventArgs args )
		{
		}

		private void Explode( GameObject root, Bounds bounds, Vector3 scale )
		{
			Vector3 adjLocalCenter = new Vector3( bounds.center.x * scale.x, bounds.center.y * scale.y, bounds.center.z * scale.z );

			Transform rootTrans = root.transform;
			Vector3 center = rootTrans.localToWorldMatrix.MultiplyPoint( adjLocalCenter );
			for ( int i = 0; i < rootTrans.childCount; i++ )
			{
				Transform pieceTrans = rootTrans.GetChild( i );
				Rigidbody body = pieceTrans.GetComponent< Rigidbody >();
				if ( body != null )
				{
					Vector3 forceVector = ( pieceTrans.position - center );
					float dist = forceVector.magnitude;

					// Normalize the vector and scale it by the explosion radius
					forceVector *= Mathf.Max( 0.0f, Radius - dist ) / ( Radius * dist );

					// Scale by the force amount
					forceVector *= Force;

					body.AddForceAtPosition( forceVector, center, ForceMode.Force );
				}
			}
		}
	}
}
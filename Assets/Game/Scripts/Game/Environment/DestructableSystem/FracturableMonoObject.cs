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
		public float ForceThreshold;
		public float ForceFalloffRadius = 1.0f;
		public bool AdjustForKinematic = true;
		public LayerMask CollidableLayers = (LayerMask)int.MaxValue;

		private bool _isFractured = false;
		private List< GameObject > _fractures = new();
		private float _thisMass;
		private Vector3 _impactPoint;
		private Vector3 _impactImpulse;
		private float _impactMass;
		private Rigidbody _impactBody;

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
			
			_thisMass = Rigidbody.mass;
			_impactPoint = Vector3.zero;
			_impactImpulse = Vector3.forward;
			
			Vector3 localPoint = transform.worldToLocalMatrix.MultiplyPoint( _impactPoint );
			FractureGeometry.FractureType = FractureType.Shatter;
			FractureGeometry.Fracture( localPoint );
		}

		private void OnCollisionEnter( Collision col )
		{
			if (!FractureGeometry.IsProcessingFracture && col.contactCount > 0)
			{
				if ((CollidableLayers.value & (1 << col.gameObject.layer)) != 0)
				{
					_impactBody = col.rigidbody;
					_impactMass = (col.rigidbody != null) ? col.rigidbody.mass : 0.0f;

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
					else
					{
						_impactMass = 0.0f;
					}
				}
			}
		}

		private void OnFracture( OnFractureEventArgs args )
		{
			if ( args.IsValid && args.OriginalObject.gameObject == gameObject && _impactMass > 0.0f )
			{
				Vector3 thisImpulse = _impactImpulse * _thisMass / ( _thisMass + _impactMass );

				for ( int i = 0; i < args.FracturePiecesRootObject.transform.childCount; i++ )
				{
					Transform piece = args.FracturePiecesRootObject.transform.GetChild( i );

					Rigidbody rb = piece.GetComponent< Rigidbody >();
					if ( rb != null )
					{
						float percentForce = FractureUtilities.GetFracturePieceRelativeMass( piece.gameObject );

						if ( ForceFalloffRadius > 0.0f )
						{
							float dist = ( piece.position - _impactPoint ).magnitude;
							percentForce *= Mathf.Clamp01( 1.0f - ( dist / ForceFalloffRadius ) );
						}

						rb.AddForce( thisImpulse * percentForce, ForceMode.Impulse );
					}
				}

				if ( AdjustForKinematic )
				{
					// If the fractured body is kinematic, the collision for the colliding body will
					// be as if it hit an unmovable wall.  Try to correct for that by adding the same
					// force to colliding body.
					if ( Rigidbody.isKinematic && _impactBody != null )
					{
						Vector3 impactBodyImpulse = _impactImpulse * _impactMass / ( _thisMass + _impactMass );
						_impactBody.AddForceAtPosition( impactBodyImpulse, _impactPoint, ForceMode.Impulse );
					}
				}
			}
		}

		private void OnFractureCompleted( OnFractureEventArgs args )
		{
			if ( args.IsValid && args.OriginalObject.gameObject == gameObject )
			{
				Debug.LogError( "OnFractureCompleted" );
			}
		}
	}
}
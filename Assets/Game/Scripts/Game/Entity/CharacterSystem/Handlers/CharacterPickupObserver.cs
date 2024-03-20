using Game.Environment.PickableSystem;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Entity.CharacterSystem
{
	public sealed class CharacterPickupObserver
	{
		public event Action< PickableObject > OnPickupped;
		public event Action< PickableObject > OnDropped;
		
		private PickableObject _pickableInLeftHand;
		private PickableObject _pickableInRightHand;
		
		private readonly CharacterPoints _points;
		
		public CharacterPickupObserver( CharacterPoints points )
		{
			_points = points ?? throw new ArgumentNullException( nameof(points) );
		}
		
		public void Pickup( PickableObject pickableObject )
		{
			if ( pickableObject is PickableLightObject )
			{
				if ( _pickableInLeftHand == null && _pickableInRightHand == null )
				{
					if ( Random.Range( 0, 2 ) == 1 )
					{
						AddToLeftHand( pickableObject );
					}
					else
					{
						AddToRightHand( pickableObject );
					}
				}
				else
				{
					if ( _pickableInRightHand != null )
					{
						AddToRightHand( pickableObject );
					}
					else
					{
						AddToLeftHand( pickableObject );
					}
				}
			}
			else if( pickableObject is PickableHeavyObject )
			{
				AddToBothHands( pickableObject );
			}
		}

		public void Drop()
		{
			if ( _pickableInLeftHand != null && _pickableInRightHand != null )
			{
				if ( _pickableInLeftHand == _pickableInRightHand )
				{
					DropObject( _pickableInLeftHand );
				}
				else
				{
					if ( Random.Range( 0, 2 ) == 1 )
					{
						DropObject( _pickableInLeftHand );
					}
					else
					{
						DropObject( _pickableInRightHand );
					}
				}
			}
			else
			{
				DropObject( _pickableInLeftHand );
				DropObject( _pickableInRightHand );
			}
		}

		public bool IsHasDrop()
		{
			return _pickableInLeftHand != null || _pickableInRightHand != null;
		}
		
		private void AddToBothHands( PickableObject pickableObject )
		{
			DropObject( _pickableInLeftHand );
			DropObject( _pickableInRightHand );
			_pickableInLeftHand = pickableObject;
			_pickableInRightHand = pickableObject;
			
			SetParent( pickableObject, _points.HeavyObjectsPoint );
			
			OnPickupped?.Invoke( pickableObject );
		}

		private void AddToLeftHand( PickableObject pickableObject )
		{
			DropObject( _pickableInLeftHand );
			_pickableInLeftHand = pickableObject;
			
			SetParent( pickableObject, _points.LeftHandPoint );
			
			OnPickupped?.Invoke( _pickableInLeftHand );
		}
		
		private void AddToRightHand( PickableObject pickableObject )
		{
			DropObject( _pickableInRightHand );
			_pickableInRightHand = pickableObject;

			SetParent( pickableObject, _points.RightHandPoint );
			
			OnPickupped?.Invoke( _pickableInRightHand );
		}

		private void SetParent( PickableObject pickableObject, Transform parent )
		{
			var trans = pickableObject.transform;
			trans.SetParent( parent );
			trans.localPosition = Vector3.zero;
			trans.position = pickableObject.InverseOffsetedPosition;
			trans.forward = parent.forward;
		}

		private void DropObject( PickableObject pickableObject )
		{
			if ( pickableObject == null ) return;

			if ( _pickableInLeftHand == pickableObject )
				_pickableInLeftHand = null;
			
			if ( _pickableInRightHand == pickableObject )
				_pickableInRightHand = null;
			
			pickableObject.transform.SetParent( null );
			pickableObject.Enable( true );
			
			OnDropped?.Invoke( pickableObject );
		}
	}
}
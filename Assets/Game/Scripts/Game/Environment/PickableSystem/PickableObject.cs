using EPOOutline;
using Game.Entity.CharacterSystem;
using Game.Systems.NavigationSystem;

using Sirenix.OdinInspector;

using StarSmithGames.Core;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Environment.PickableSystem
{
	public abstract class PickableObject : EnvironmentObject, IEnableable
	{
		public bool IsEnable { get; private set; } = true;

		public Vector3 OffsetedPosition => transform.position + PositionOffset;
		public Vector3 InverseOffsetedPosition => transform.position - PositionOffset;

		public ItemCanvas ItemCanvas;
		public Rigidbody Rigidbody;
		public CharacterInteractionZone InteractionZone;
		public Outlinable Outlinable;
		public List<Collider> Colliders = new();
		[ Space ]
		public Vector3 PositionOffset;

		private Character _currentCharacter;
		
		private void Awake()
		{
			InteractionZone.OnItemAdded += CharacterEnterZoneHandler;
			InteractionZone.OnItemRemoved += CharacterExitZoneHandler;

			ItemCanvas.Pickup.OnButtonClicked += PickUpButtonClickedHandler;
			ItemCanvas.Pickup.Enable( false );
		}

		private void OnDestroy()
		{
			if (InteractionZone != null)
			{
				InteractionZone.OnItemAdded -= CharacterEnterZoneHandler;
				InteractionZone.OnItemRemoved -= CharacterExitZoneHandler;
			}
			
			ItemCanvas.Pickup.OnButtonClicked -= PickUpButtonClickedHandler;
		}
		
		public void Enable(bool trigger)
		{
			Colliders.ForEach((x) => x.enabled = trigger);
			Rigidbody.isKinematic = !trigger;
			Rigidbody.useGravity = trigger;
			Outlinable.enabled = trigger;

			ItemCanvas.gameObject.SetActive( trigger );
			
			IsEnable = trigger;
		}

		private void PickupAnimation()
		{
			ItemCanvas.Pickup.Enable( false );
			Enable( false );
		}

		private void EnterAnimation()
		{
			ItemCanvas.Pickup.Show();
		}

		private void ResetAnimation()
		{
			ItemCanvas.Pickup.Hide();
		}

		protected virtual void CharacterEnterZoneHandler( Character character )
		{
			if ( !IsEnable ) return;
			
			_currentCharacter = character;
			
			EnterAnimation();
		}

		protected virtual void CharacterExitZoneHandler( Character character )
		{
			if ( !IsEnable ) return;
			
			_currentCharacter = null;
			
			ResetAnimation();
		}

		private void PickUpButtonClickedHandler()
		{
			PickupAnimation();
			ResetAnimation();
			
			_currentCharacter.Presenter.PickupObserver.Pickup( this );
			_currentCharacter = null;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere( OffsetedPosition, 0.1f);
		}
	}
}
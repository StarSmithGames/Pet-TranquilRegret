using EPOOutline;
using Game.Entity.CharacterSystem;
using Game.Systems.NavigationSystem;

using Sirenix.OdinInspector;

using StarSmithGames.Core;
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Environment.PickableSystem
{
	public abstract class PickableObject : EnvironmentObject, IEnableable
	{
		public bool IsEnable { get; private set; } = true;

		public Vector3 OffsetedPosition => transform.position + PositionOffset;
		public Vector3 InverseOffsetedPosition => transform.position - PositionOffset;

		public Rigidbody Rigidbody;
		public CharacterInteractionZone InteractionZone;
		public Outlinable Outlinable;
		public List<Collider> Colliders = new();
		[ Space ]
		public Vector3 PositionOffset;

		private ItemCanvas ItemCanvas
		{
			get
			{
				if ( _itemCanvas == null )
				{
					_itemCanvas = _itemCanvasFactory.Create();
					_itemCanvas.transform.SetParent( transform );
					_itemCanvas.transform.localScale = Vector3.one;
					_itemCanvas.transform.localPosition = Vector3.zero;
					_itemCanvas.Pickup.OnButtonClicked += PickUpButtonClickedHandler;
					_itemCanvas.Pickup.Enable( false );
				}

				return _itemCanvas;
			}
		}
		private ItemCanvas _itemCanvas;

		private Character _currentCharacter;

		private ItemCanvas.Factory _itemCanvasFactory;
		
		[ Inject ]
		private void Construct( ItemCanvas.Factory itemCanvasFactory )
		{
			_itemCanvasFactory = itemCanvasFactory ?? throw new ArgumentNullException( nameof(itemCanvasFactory) );
		}
		
		private void Awake()
		{
			InteractionZone.OnItemAdded += CharacterEnterZoneHandler;
			InteractionZone.OnItemRemoved += CharacterExitZoneHandler;
		}

		private void OnDestroy()
		{
			if (InteractionZone != null)
			{
				InteractionZone.OnItemAdded -= CharacterEnterZoneHandler;
				InteractionZone.OnItemRemoved -= CharacterExitZoneHandler;
			}

			DisposePickup();
		}
		
		private void DisposePickup()
		{
			if ( _itemCanvas == null ) return;
			
			_itemCanvas.DespawnIt();
			_itemCanvas.Pickup.OnButtonClicked -= PickUpButtonClickedHandler;
			_itemCanvas = null;
		}
		
		public void Enable(bool trigger)
		{
			Colliders.ForEach((x) => x.enabled = trigger);
			Rigidbody.isKinematic = !trigger;
			Rigidbody.useGravity = trigger;
			Outlinable.enabled = trigger;

			if ( trigger )
			{
				ItemCanvas.Pickup.Enable( true );
			}
			else
			{
				DisposePickup();
			}
			
			IsEnable = trigger;
		}

		private void PickupAnimation()
		{
			Enable( false );
		}

		private void EnterAnimation()
		{
			ItemCanvas.Pickup.Show();
		}

		private void ResetAnimation()
		{
			ItemCanvas.Pickup.Hide( DisposePickup );
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
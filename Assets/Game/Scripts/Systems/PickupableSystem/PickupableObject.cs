using EPOOutline;

using Game.Character;
using Game.Entities;
using Game.Systems.NavigationSystem;

using Sirenix.OdinInspector;

using StarSmithGames.Core;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Game.Systems.PickupableSystem
{
	public class PickupableObject : MonoBehaviour, IEnableable
	{
		public bool IsEnable { get; private set; } = true;

		public bool IsInteractable { get; private set; } = true;

		public Vector3 PositionOffset => transform.position + settings.positionOffset;

		public Rigidbody Rigidbody => rigidbody;
		[SerializeField] protected Rigidbody rigidbody;
		[SerializeField] protected InteractionZone interactionZone;
		[SerializeField] protected Outlinable outlinable;
		[SerializeField] protected List<Collider> colliders = new List<Collider>();
		[SerializeField] protected Settings settings;

		private Character.Character lastPlayer;
		private Character.Character player;
		private UIPickup currentPickup;

		//private UIPickup.Factory pickupFactory;

		//[Inject]
		//private void Construct(UIPickup.Factory pickupFactory)
		//{
		//	this.pickupFactory = pickupFactory;
		//}

		//private void Start()
		//{
		//	interactionZone.onEnterChanged += OnEnterChanged;
		//	interactionZone.onExitChanged += OnExitChanged;
		//}

		//private void OnDestroy()
		//{
		//	if (interactionZone != null)
		//	{
		//		interactionZone.onEnterChanged -= OnEnterChanged;
		//		interactionZone.onExitChanged -= OnExitChanged;
		//	}
		//}

		private void FixedUpdate()
		{
			if (rigidbody.velocity.y < 0)
			{
				rigidbody.velocity += Vector3.up * Physics.gravity.y * settings.fallMultipier * Time.fixedDeltaTime;
			}
		}

		public void Enable(bool trigger)
		{
			colliders.ForEach((x) => x.enabled = trigger);
			rigidbody.isKinematic = !trigger;
			rigidbody.useGravity = trigger;

			IsEnable = trigger;
		}

		public void EnableInteract(bool trigger)
		{
			interactionZone.Enable(trigger);
			IsInteractable = trigger;
		}

		private void PickupAnimation()
		{
			
		}

		private void EnterAnimation()
		{
			//currentPickup = pickupFactory.Create();
			//currentPickup.onClicked += OnPickuped;
			//currentPickup.Show(this);
		}

		private void ResetAnimation()
		{
			if (currentPickup != null)
			{
				//currentPickup.onClicked -= OnPickuped;
				//currentPickup.Hide();
				//currentPickup = null;
			}
		}

		private void OnPickuped()
		{
			PickupAnimation();
			ResetAnimation();

			//lastPlayer.Pickup(this);
			lastPlayer = player;
			player = null;
		}
	
		protected virtual void OnEnterChanged(Collider other)
		{
			//var p = other.GetComponentInParent<Character>();

			//if (p != null)
			//{
			//	player = p;
			//	lastPlayer = player;

			//	EnterAnimation();
			//}
		}

		protected virtual void OnExitChanged(Collider other)
		{
			//var p = other.GetComponentInParent<Character>();

			//if (p == player)
			//{
			//	ResetAnimation();

			//	lastPlayer = player;
			//	player = null;
			//}
		}

		[Button(DirtyOnClick = true)]
		private void FillColliders()
		{
			colliders = GetComponentsInChildren<Collider>().ToList();
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(transform.position + settings.positionOffset, 0.2f);
		}

		[System.Serializable]
		public class Settings
		{
			public Vector3 positionOffset;
			public float fallMultipier = 3f;
		}
	}
}
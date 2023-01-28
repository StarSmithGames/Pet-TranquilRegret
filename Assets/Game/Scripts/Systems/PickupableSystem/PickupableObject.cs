using EPOOutline;

using Game.Entities;
using Game.Systems.InteractionSystem;

using Sirenix.OdinInspector;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;

namespace Game.Systems.PickupableSystem
{
	public class PickupableObject : MonoBehaviour
	{
		public Vector3 PositionOffset => transform.position + settings.positionOffset;

		[SerializeField] protected Rigidbody rigidbody;
		[SerializeField] protected InteractionZone interactionZone;
		[SerializeField] protected Outlinable outlinable;
		[SerializeField] protected List<Collider> colliders = new List<Collider>();
		[SerializeField] protected Settings settings;

		private Player lastPlayer;
		private Player player;
		private UIPickup currentPickup;

		private UIPickup.Factory pickupFactory;

		[Inject]
		private void Construct(UIPickup.Factory pickupFactory)
		{
			this.pickupFactory = pickupFactory;
		}

		private void Start()
		{
			interactionZone.onCollectionChanged += OnZoneCollectionChanged;
		}

		private void OnDestroy()
		{
			if (interactionZone != null)
			{
				interactionZone.onCollectionChanged -= OnZoneCollectionChanged;
			}
		}

		public void Enable(bool trigger)
		{
			interactionZone.Enable(trigger);
			colliders.ForEach((x) => x.enabled = trigger);
			rigidbody.isKinematic = !trigger;
			rigidbody.useGravity = trigger;
		}


		private void StartAnimation()
		{
			//decalVFX.Kill();
			//decalVFX.ScaleTo(1.25f);
			//itemCanvas.Pickup.Show();

			currentPickup = pickupFactory.Create();
			currentPickup.onClicked += OnPickuped;
			currentPickup.Show(this);
		}

		private void ResetAnimation()
		{
			//decalVFX.ScaleTo(1f, callback: decalVFX.StartIdleAnimation);
			//itemCanvas.Pickup.Hide();

			if (currentPickup != null)
			{
				currentPickup.onClicked -= OnPickuped;
				currentPickup.Hide();
				currentPickup = null;
			}
		}

		private void OnPickuped()
		{
			Enable(false);

			lastPlayer.Pickup(this);
		}
	
		private void OnZoneCollectionChanged()
		{
			lastPlayer = player;
			player = interactionZone.GetCollection().FirstOrDefault()?.GetComponentInParent<Player>();

			if (player != null)
			{
				StartAnimation();
			}
			else
			{
				ResetAnimation();
			}
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
		}
	}
}
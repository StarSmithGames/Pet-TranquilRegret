using DG.Tweening;

using Game.Managers.CharacterManager;
using Game.Systems.PickupableSystem;
using Game.VFX.Markers;

using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Entities
{
	public class HeavyDropZone : DropZone
	{
		[SerializeField] private Marker marker;

		private List<PickupableObject> pickupables = new List<PickupableObject>();

		private CharacterManager characterManager;

		[Inject]
		private void Constrct(CharacterManager characterManager)
		{
			this.characterManager = characterManager;
		}

		protected override void Start()
		{
			base.Start();

			characterManager.CurrentPlayer.onObjectInHandsChanged += OnObjectInPlayerHandsChanged;
		}

		protected override void ResetAnimation()
		{
			if (characterManager.CurrentPlayer.IsHandsEmpty)
			{
				decal.ScaleTo(1f);
			}
			else
			{
				base.ResetAnimation();
			}
		}

		private void OnObjectInPlayerHandsChanged(PickupableObject pickupable)
		{
			marker.Enable(pickupable != null);
			if (pickupable != null)
			{
				IdleAnimation();
			}
			else
			{
				KillIdleAnimation();
			}
		}

		protected override void OnEnterChanged(Collider other)
		{
			base.OnEnterChanged(other);

			var pickupable = other.GetComponentInParent<PickupableObject>();

			if (pickupable != null)
			{
				if (!pickupables.Contains(pickupable))
				{
					pickupable.EnableInteract(false);
					pickupables.Add(pickupable);

					Sequence sequence = DOTween.Sequence();

					sequence
						.AppendInterval(2f)
						.Append(pickupable.transform.DOScale(0, 0.2f))
						.OnComplete(() =>
						{
							pickupables.Remove(pickupable);
							Destroy(pickupable.gameObject);
						});
				}
			}
		}
	}
}
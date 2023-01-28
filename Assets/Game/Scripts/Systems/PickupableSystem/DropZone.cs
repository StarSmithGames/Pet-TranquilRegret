using DG.Tweening;

using Game.Entities;
using Game.Managers.CharacterManager;
using Game.Systems.InteractionSystem;
using Game.VFX.Markers;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;

namespace Game.Systems.PickupableSystem
{
	public class DropZone : InteractableZoneObject
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

			characterManager.CurrentPlayer.onObjectInHandsChanged += onObjectInPlayerHandsChanged;
		}

		private void onObjectInPlayerHandsChanged(PickupableObject pickupable)
		{
			marker.Enable(pickupable != null);
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
							Debug.LogError("Chpok");

							pickupables.Remove(pickupable);
							Destroy(pickupable.gameObject);
						});
				}
			}
		}
	}
}
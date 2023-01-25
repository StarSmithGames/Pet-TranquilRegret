using EPOOutline;

using Game.Entities;
using Game.Systems.InteractionSystem;
using Game.VFX;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Game.Systems.PickupableSystem
{
	public class PickupableObject : MonoBehaviour
	{
		[SerializeField] protected InteractionZone interactionZone;
		[SerializeField] protected Outlinable outlinable;

		private Player lastPlayer;
		private Player player;

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

		private void StartAnimation()
		{
			//decalVFX.Kill();
			//decalVFX.ScaleTo(1.25f);
			//itemCanvas.Pickup.Show();
		}

		private void ResetAnimation()
		{
			//decalVFX.ScaleTo(1f, callback: decalVFX.StartIdleAnimation);
			//itemCanvas.Pickup.Hide();
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
	}
}
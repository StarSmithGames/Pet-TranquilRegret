using Game.Entities;
using Game.Systems.InteractionSystem;
using Game.VFX;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Game.Systems.PickupableSystem
{
	public class DropZone : MonoBehaviour
	{
		[SerializeField] private DecalVFX decal;
		[SerializeField] private InteractionZone interactionZone;

		private Player lastPlayer;
		private Player player;

		private void Start()
		{
			interactionZone.onCollectionChanged += OnInteractionCollectionChanged;

			decal.StartIdleAnimation();
		}

		private void OnDestroy()
		{
			if (interactionZone != null)
			{
				interactionZone.onCollectionChanged -= OnInteractionCollectionChanged;
			}
		}

		private void OnInteractionCollectionChanged()
		{
			lastPlayer = player;
			player = interactionZone.GetCollection().FirstOrDefault()?.GetComponentInParent<Player>();
		}
	}
}
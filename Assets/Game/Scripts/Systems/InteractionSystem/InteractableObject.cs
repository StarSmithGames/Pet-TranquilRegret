using Game.Entities;
using Game.VFX;

using UnityEngine;

namespace Game.Systems.InteractionSystem
{
	public class InteractableObject : MonoBehaviour
	{
		[SerializeField] protected DecalVFX decal;
		[SerializeField] protected InteractionZone interactionZone;

		protected Player lastPlayer;
		protected Player player;

		protected virtual void Start()
		{
			interactionZone.onEnterChanged += OnEnterChanged;
			interactionZone.onExitChanged += OnExitChanged;
			interactionZone.onCollectionChanged += OnZoneCollectionChanged;
		}

		protected virtual void OnDestroy()
		{
			if (interactionZone != null)
			{
				interactionZone.onEnterChanged -= OnEnterChanged;
				interactionZone.onExitChanged -= OnExitChanged;
				interactionZone.onCollectionChanged -= OnZoneCollectionChanged;
			}
		}

		protected virtual void IdleAnimation()
		{
			decal.StartIdleAnimation();
		}

		protected virtual void KillIdleAnimation()
		{
			decal.Kill();
		}

		protected virtual void EnterAnimation()
		{
			KillIdleAnimation();
			decal.ScaleTo(1.2f);
		}

		protected virtual void ResetAnimation()
		{
			decal.ScaleTo(1f, callback: decal.StartIdleAnimation);
		}

		protected virtual void OnPlayerEnter(Player player) { }

		protected virtual void OnPlayerExit(Player player) { }

		protected virtual void OnEnterChanged(Collider other)
		{
			var p = other.GetComponentInParent<Player>();

			if(p != null)
			{
				player = p;
				lastPlayer = player;

				EnterAnimation();

				OnPlayerEnter(player);
			}
		}

		protected virtual void OnExitChanged(Collider other)
		{
			var p = other.GetComponentInParent<Player>();

			if(p == player)
			{
				ResetAnimation();

				lastPlayer = player;
				player = null;

				OnPlayerExit(lastPlayer);
			}
		}

		protected virtual void OnZoneCollectionChanged() { }
	}
}
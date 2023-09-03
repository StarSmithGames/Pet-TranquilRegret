using Game.Character;
using Game.Entities;
using Game.VFX;

using UnityEngine;

namespace Game.Systems.InteractionSystem
{
	public class InteractableObject : MonoBehaviour
	{
		[SerializeField] protected DecalVFX decal;
		[SerializeField] protected InteractionZone interactionZone;

		protected Character.Character lastPlayer;
		protected Character.Character player;

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

		protected virtual void OnPlayerEnter(Character.Character player) { }

		protected virtual void OnPlayerExit(Character.Character player) { }

		protected virtual void OnEnterChanged(Collider other)
		{
			//var p = other.GetComponentInParent<Character>();

			//if(p != null)
			//{
			//	player = p;
			//	lastPlayer = player;

			//	EnterAnimation();

			//	OnPlayerEnter(player);
			//}
		}

		protected virtual void OnExitChanged(Collider other)
		{
			//var p = other.GetComponentInParent<Character>();

			//if(p == player)
			//{
			//	ResetAnimation();

			//	lastPlayer = player;
			//	player = null;

			//	OnPlayerExit(lastPlayer);
			//}
		}

		protected virtual void OnZoneCollectionChanged() { }
	}
}
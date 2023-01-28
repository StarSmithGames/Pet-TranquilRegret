using Game.Entities;
using Game.VFX;
using System.Linq;

using UnityEngine;

namespace Game.Systems.InteractionSystem
{
	public class InteratableZoneObject : MonoBehaviour
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

			IdleAnimation();
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

		protected virtual void EnterAnimation()
		{
			decal.Kill();
			decal.ScaleTo(1.2f);
		}

		protected virtual void ResetAnimation()
		{
			decal.ScaleTo(1f, callback: decal.StartIdleAnimation);
		}

		protected virtual void OnEnterChanged(Collider other)
		{
			var p = other.GetComponentInParent<Player>();

			if(p != null)
			{
				player = p;
				lastPlayer = player;

				EnterAnimation();
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
			}
		}

		protected virtual void OnZoneCollectionChanged() { }
	}
}
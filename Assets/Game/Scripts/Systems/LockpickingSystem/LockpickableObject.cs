using Game.Entities;
using Game.Systems.InteractionSystem;
using Game.VFX;
using System.Linq;

using UnityEngine;

namespace Game.Systems.LockpickingSystem
{
	public class LockpickableObject : MonoBehaviour
	{
		[SerializeField] protected InteractionZone interactionZone;
		[SerializeField] protected DecalVFX decalVFX;
		[SerializeField] protected Settings settings;

		private Player lastPlayer;
		private Player player;
		private float t;
		private float progress;

		protected virtual void Start()
		{
			decalVFX.Enable(settings.isLocked);

			if (settings.isLocked)
			{
				IdleAnimation();
			}

			interactionZone.onCollectionChanged += OnZoneCollectionChanged;
		}

		private void OnDestroy()
		{
			if(interactionZone != null)
			{
				interactionZone.onCollectionChanged -= OnZoneCollectionChanged;
			}
		}

		private void Update()
		{
			if (!settings.isLocked) return;

			if(player != null)
			{
				t += Time.deltaTime;
				progress = t / settings.unlockTime;

				player.PlayerCanvas.Lockpick.FillAmount = progress;

				if (progress >= 1f)
				{
					settings.isLocked = false;
					OnLockChanged();
				}
			}
			else
			{
				if (progress != 0)
				{
					t -= Time.deltaTime * settings.decreaseSpeed;
					progress = Mathf.Max(t, 0) / settings.unlockTime;
				}
			}
		}

		private void UnlockAnimation()
		{
			decalVFX.ScaleTo(0f);
			player.PlayerCanvas.Lockpick.Unlock();
		}

		private void IdleAnimation()
		{
			decalVFX.StartIdleAnimation();
		}

		private void StartAnimation()
		{
			decalVFX.Kill();
			decalVFX.ScaleTo(1.25f);
			player.PlayerCanvas.Lockpick.Show();
		}

		private void ResetAnimation()
		{
			decalVFX.ScaleTo(1f, callback: decalVFX.StartIdleAnimation);
			lastPlayer.PlayerCanvas.Lockpick.Hide();
		}

		protected virtual void OnLockChanged()
		{
			UnlockAnimation();
		}

		private void OnZoneCollectionChanged()
		{
			if (!settings.isLocked) return;

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

		[System.Serializable]
		public class Settings
		{
			public bool isLocked = true;
			public float unlockTime = 2.5f;
			public float decreaseSpeed = 1f;
		}
	}
}
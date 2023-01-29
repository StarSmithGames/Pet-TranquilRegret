using Game.Systems.InteractionSystem;

using UnityEngine;

namespace Game.Systems.LockpickingSystem
{
	public class LockpickableObject : InteractableObject
	{
		[SerializeField] protected Settings settings;

		private float t;
		private float progress;

		protected override void Start()
		{
			decal.Enable(settings.isLocked);

			if (settings.isLocked)
			{
				IdleAnimation();
			}

			interactionZone.onEnterChanged += OnEnterChanged;
			interactionZone.onExitChanged += OnExitChanged;
			interactionZone.onCollectionChanged += OnZoneCollectionChanged;
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
			decal.ScaleTo(0f);
			player.PlayerCanvas.Lockpick.Unlock();
		}

		#region Override
		protected override void EnterAnimation()
		{
			base.EnterAnimation();
			player.PlayerCanvas.Lockpick.Show();
		}

		protected override void ResetAnimation()
		{
			base.ResetAnimation();
			lastPlayer.PlayerCanvas.Lockpick.Hide();
		}

		protected override void OnEnterChanged(Collider other)
		{
			if (!settings.isLocked) return;

			base.OnEnterChanged(other);
		}

		protected override void OnExitChanged(Collider other)
		{
			if (!settings.isLocked) return;

			base.OnExitChanged(other);
		}

		protected override void OnZoneCollectionChanged()
		{
			if (!settings.isLocked) return;

			base.OnZoneCollectionChanged();
		}
		#endregion

		protected virtual void OnLockChanged()
		{
			UnlockAnimation();
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
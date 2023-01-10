using DG.Tweening;

using Game.Systems.InteractionSystem;
using Game.VFX;
using System.Linq;

using UnityEngine;

namespace Game.Systems.LockpickingSystem
{
	public class LockpickableObject : MonoBehaviour
	{
		[SerializeField] private InteractionZone interactionZone;
		[SerializeField] private DecalVFX decalVFX;
		[SerializeField] private Settings settings;

		private Player lastPlayer;
		private Player player;
		private float t;
		private float progress;

		private void Start()
		{
			if (!settings.isLocked)
			{
				decalVFX.Enable(false);
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

				player.PlayerCanvas.LockpickBar.FillAmount = progress;

				if (progress >= 1f)
				{
					settings.isLocked = false;
					UnlockAnimation();
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
			player.PlayerCanvas.LockpickBar.Unlock();
		}

		private void StartAnimation()
		{
			decalVFX.ScaleTo(1.25f);
			player.PlayerCanvas.LockpickBar.Show();
		}

		private void ResetAnimation()
		{
			decalVFX.ScaleTo(1f);
			lastPlayer.PlayerCanvas.LockpickBar.Hide();
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
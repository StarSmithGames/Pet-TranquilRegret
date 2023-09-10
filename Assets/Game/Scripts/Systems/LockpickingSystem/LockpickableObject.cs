using Game.Character;
using Game.Systems.InteractionSystem;
using Game.Systems.NavigationSystem;

using Sirenix.OdinInspector;

using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Game.Systems.LockpickingSystem
{
	public class LockpickableObject : InteractableObject
	{
		public Action<LockpickableObject> onLockChanged;

		[InfoBox("Overrided by Group Parent", InfoMessageType.Error, VisibleIf = "IsHasLockpickableGroup")]
		public LockpickableSettings settings;
		[SerializeField, ReadOnly] private LockpickableGroup group;

		private AbstractCharacter currentCharacter;

		private float t;
		private float progress;

		protected override void Awake()
		{
			base.Awake();

			if(group != null)
			{
				settings = group.settings;
			}

			interactionZone.decal.Enable(settings.isLocked);
			if (settings.isLocked)
			{
				interactionZone.DoIdle();
			}
		}

		private void Update()
		{
			if (!settings.isLocked) return;

			if(currentCharacter != null)
			{
				t += Time.deltaTime;
				progress = t / settings.unlockTime;

				currentCharacter.facade.characterCanvas.lockpick.FillAmount = progress;

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

		public void Hide()
		{
			interactionZone.Enable(false);
			interactionZone.decal.ScaleTo(0f, callback: () =>
			{
				interactionZone.gameObject.SetActive(false);
			});
		}

		public void DoUnlock()
		{
			Hide();
			currentCharacter.facade.characterCanvas.lockpick.Unlock();
		}

		protected virtual void OnLockChanged()
		{
			onLockChanged?.Invoke(this);
		}

		protected override void OnListChanged()
		{
			var character = characters.LastOrDefault();

			if (currentCharacter != null)
			{
				if (character != currentCharacter)
				{
					currentCharacter.facade.characterCanvas.lockpick.Hide();
				}
			}

			currentCharacter = character;

			if (currentCharacter != null)
			{
				currentCharacter.facade.characterCanvas.lockpick.Show();
			}
		}


#if UNITY_EDITOR
		private bool IsHasLockpickableGroup()
		{
			var group = GetComponentInParent<LockpickableGroup>();

			if(group != this.group)
			{
				this.group = group;
				EditorUtility.SetDirty(gameObject);
			}

			return group != null;
		}
#endif
	}
}
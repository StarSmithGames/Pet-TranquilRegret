using Game.Environment.InteractionSystem;
using System.Collections.Generic;
using Game.Systems.NavigationSystem;
using Game.Systems.ZoneSystem;

namespace Game.Environment.LockpickingSystem
{
	public abstract partial class LockpickableObject : InteractableObject
	{
		public LockpickableSettings settings;

		public List<CharacterInteractionZone> zones = new();

		private float t;
		private float progress;

		protected virtual void Awake()
		{
			if (settings.isLocked)
			{
				Lock();
			}
			else
			{
				UnLock();
			}
		}

		private void OnDestroy()
		{
			UnSubscribe();
		}

		private void Lock()
		{
			Subscribe();

			EnableZones(true);
		}

		private void UnLock()
		{
			UnSubscribe();

			EnableZones(false);
		}

		private void EnableZones(bool trigger)
		{
			for (int i = 0; i < zones.Count; i++)
			{
				zones[i].gameObject.SetActive(trigger);
			}
		}

		protected virtual void OnCharacterAdded(IZonable item) 
		{
			(item as Entity.CharacterSystem.Character).Presenter.DoLockpickAsync();
		}

		protected virtual void OnCharacterRemoved(IZonable item)
		{
			(item as Entity.CharacterSystem.Character).Presenter.BreakLockpick();
		}
	}

	public partial class LockpickableObject
	{
		private void Subscribe()
		{
			for (int i = 0; i < zones.Count; i++)
			{
				zones[i].OnItemAdded += OnCharacterAdded;
				zones[i].OnItemRemoved += OnCharacterRemoved;
			}
		}

		private void UnSubscribe()
		{
			for (int i = 0; i < zones.Count; i++)
			{
				zones[i].OnItemAdded -= OnCharacterAdded;
				zones[i].OnItemRemoved -= OnCharacterRemoved;
			}
		}
	}
}
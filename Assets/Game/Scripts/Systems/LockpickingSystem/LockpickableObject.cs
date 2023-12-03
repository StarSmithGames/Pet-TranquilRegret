using System.Collections.Generic;
using Game.Systems.NavigationSystem;
using Game.Systems.InteractionSystem;
using Game.Systems.ZoneSystem;

namespace Game.Systems.LockpickingSystem
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
			(item as Character.Character).Presenter.DoLockpickAsync();
		}

		protected virtual void OnCharacterRemoved(IZonable item)
		{
			(item as Character.Character).Presenter.BreakLockpick();
		}
	}

	public partial class LockpickableObject
	{
		private void Subscribe()
		{
			for (int i = 0; i < zones.Count; i++)
			{
				zones[i].onItemAdded += OnCharacterAdded;
				zones[i].onItemRemoved += OnCharacterRemoved;
			}
		}

		private void UnSubscribe()
		{
			for (int i = 0; i < zones.Count; i++)
			{
				zones[i].onItemAdded -= OnCharacterAdded;
				zones[i].onItemRemoved -= OnCharacterRemoved;
			}
		}
	}
}
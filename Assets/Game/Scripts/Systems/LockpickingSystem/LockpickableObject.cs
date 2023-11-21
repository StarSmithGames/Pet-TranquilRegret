using System.Collections.Generic;
using System.Linq;


using UnityEngine;
using Game.Systems.NavigationSystem;
using Game.Systems.InteractionSystem;
using Game.Character;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
#endif

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
		}

		private void OnDestroy()
		{

		}

		private void Lock()
		{
			Subscribe();
		}

		private void UnLock()
		{
			UnSubscribe();
		}

		protected virtual void OnCharacterAdded(Character.Character character) 
		{
			character.Presenter.DoLockpickAsync();
		}

		protected virtual void OnCharacterRemoved(Character.Character character)
		{
			character.Presenter.BreakLockpick();
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
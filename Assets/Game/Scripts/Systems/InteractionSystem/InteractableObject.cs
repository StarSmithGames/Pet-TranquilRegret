using Game.Character;
using Game.Systems.NavigationSystem;

using System.Collections.Generic;

using UnityEngine;

namespace Game.Systems.InteractionSystem
{
	public class InteractableObject : MonoBehaviour
	{
		public InteractionZone interactionZone;

		protected List<AbstractCharacter> characters = new();

		protected virtual void Awake()
		{
			interactionZone.Registrator.onItemAdded += OnEnterChanged;
			interactionZone.Registrator.onItemRemoved += OnExitChanged;
		}

		protected virtual void OnDestroy()
		{
			if (interactionZone != null)
			{
				interactionZone.Registrator.onItemAdded -= OnEnterChanged;
				interactionZone.Registrator.onItemRemoved -= OnExitChanged;
			}
		}

		protected virtual void OnListChanged() { }

		private void OnEnterChanged(Collider other)
		{
			var character = other.GetComponentInParent<AbstractCharacter>();

			if (character == null) return;
			if (characters.Contains(character)) return;

			characters.Add(character);
			interactionZone.DoEnter();

			OnListChanged();
		}

		private void OnExitChanged(Collider other)
		{
			var character = other.GetComponentInParent<AbstractCharacter>();

			if (characters.Contains(character))
			{
				interactionZone.ResetAnimation();

				characters.Remove(character);

				OnListChanged();
			}
		}
	}
}
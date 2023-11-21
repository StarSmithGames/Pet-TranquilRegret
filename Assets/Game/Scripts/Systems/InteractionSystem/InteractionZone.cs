using Game.Extensions;

using StarSmithGames.Core;

using System;

using UnityEngine;

namespace Game.Systems.InteractionSystem
{
	public abstract class InteractionZone<T> : MonoBehaviour where T : MonoBehaviour
	{
		public event Action<T> onItemAdded;
		public event Action<T> onItemRemoved;

		public Registrator<T> Registrator { get; } = new();


		protected virtual void OnTriggerEnter(Collider other)
		{
			var item = other.FindTopmostComponent<T>();
			if (item == null) return;

			if (!Registrator.registers.Contains(item))
			{
				Registrator.Registrate(item);
				OnItemAdded(item);
				onItemAdded?.Invoke(item);
			}
		}

		protected virtual void OnTriggerExit(Collider other)
		{
			var item = other.FindTopmostComponent<T>();
			if (item == null) return;

			if (Registrator.registers.Contains(item))
			{
				Registrator.UnRegistrate(item);
				OnItemRemoved(item);
				onItemRemoved?.Invoke(item);
			}
		}

		protected abstract void OnItemAdded(T item);
		protected abstract void OnItemRemoved(T item);
	}
}
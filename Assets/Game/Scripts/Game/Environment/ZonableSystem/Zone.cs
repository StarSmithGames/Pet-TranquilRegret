using StarSmithGames.Core;

using UnityEngine;

namespace Game.Systems.ZoneSystem
{
	public abstract class Zone : MonoBehaviour
	{
		public Registrator<IZonable> Registrator { get; } = new();

		public abstract Collider Collider { get; }


		protected virtual void OnTriggerEnter(Collider other)
		{
			var item = other.transform.root.GetComponent<IZonable>();
			if (item == null) return;

			if (!Registrator.registers.Contains(item))
			{
				Registrator.Registrate(item);
				OnItemAdded(item);
			}
		}

		protected virtual void OnTriggerExit(Collider other)
		{
			var item = other.transform.root.GetComponent<IZonable>();
			if (item == null) return;

			if (Registrator.registers.Contains(item))
			{
				Registrator.UnRegistrate(item);
				OnItemRemoved(item);
			}
		}


		protected virtual void OnItemAdded(IZonable item) { }
		protected virtual void OnItemRemoved(IZonable item) { }
	}
}
using StarSmithGames.Core;
using System;
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
				ItemAddedHandler(item);
			}
		}

		protected virtual void OnTriggerExit(Collider other)
		{
			var item = other.transform.root.GetComponent<IZonable>();
			if (item == null) return;

			if (Registrator.registers.Contains(item))
			{
				Registrator.UnRegistrate(item);
				ItemRemovedHandler(item);
			}
		}

		protected virtual void ItemAddedHandler(IZonable item) { }
		protected virtual void ItemRemovedHandler(IZonable item) { }
	}
}
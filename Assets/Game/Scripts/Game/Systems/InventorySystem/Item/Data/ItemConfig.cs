using UnityEngine;

namespace Game.Systems.InventorySystem
{
	public abstract class ItemConfig : ScriptableObject
	{
		public int id;
		public string name;

		public string BaseName => base.name;
	}
}
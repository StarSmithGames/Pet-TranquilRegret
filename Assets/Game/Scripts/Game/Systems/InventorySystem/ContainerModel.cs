using System.Collections.Generic;

namespace Game.Systems.InventorySystem
{
	public class ContainerModel
	{
		public InventoryModel Inventory { get; }

		public ContainerModel(ItemModel[] items)
		{
			Inventory = new();
			Inventory.Registrate(items);
		}
	}
}
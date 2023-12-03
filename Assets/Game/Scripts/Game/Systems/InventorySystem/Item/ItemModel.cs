using StarSmithGames.Core;

namespace Game.Systems.InventorySystem
{
	[System.Serializable]
	public class ItemModel : ICopyable<ItemModel>
	{
		public ItemConfig config;

		public ItemModel Copy()
		{
			return new()
			{ 
				config = config
			};
		}
	}
}
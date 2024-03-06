using Game.Systems.InventorySystem;

using UnityEngine;

namespace Game.Environment.InteractionSystem
{
    public class Coin : ItemView
	{
		[Min(1)]
		public int count = 1;
	}
}
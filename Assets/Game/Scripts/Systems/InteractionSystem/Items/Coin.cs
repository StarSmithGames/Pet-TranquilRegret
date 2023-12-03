using UnityEngine;

namespace Game.Systems.InteractionSystem
{
    public class Coin : InteractableItem
	{
		[Min(1)]
		public int count = 1;
	}
}
using UnityEngine;

namespace Game.Managers.LayerManager
{
	public static class Layers
	{
		public static int LAYER_GROUND => LayerMask.GetMask("Ground");
		public static int LAYER_ENVIRONMENT => LayerMask.GetMask("Environment");
		public static int LAYER_CHARACTER => LayerMask.GetMask("Character");

		public static bool IsContains(int layer, int layers)
		{
			return layer == (layer | (1 << layers));
		}
	}
}
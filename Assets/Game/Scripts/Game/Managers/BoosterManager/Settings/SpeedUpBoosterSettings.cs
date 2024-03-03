using UnityEngine;

namespace Game.Systems.BoosterManager.Settings
{
	[ System.Serializable ]
	public sealed class SpeedUpBoosterSettings
	{
		[ Min(0f) ]
		public float EstimatedTime = 10f;

		[ Min(1f) ]
		public float SpeedMultiplier = 2f;
	}
}
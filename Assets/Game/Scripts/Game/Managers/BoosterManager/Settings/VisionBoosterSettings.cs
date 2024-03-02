using EPOOutline;
using UnityEngine;

namespace Game.Systems.BoosterManager.Settings
{
	[ System.Serializable ]
	public sealed class VisionBoosterSettings
	{
		[ Min(0f) ]
		public float EstimatedTime = 30f;
		
		public OutlineData Outline;
	}
}
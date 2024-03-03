using EPOOutline;
using UnityEngine;

namespace Game.Systems.BoosterManager.Settings
{
	[ System.Serializable ]
	public sealed class VisionBoosterSettings
	{
		[ Min(0) ]
		public int CountOnStart = 1;
		[ Space ]
		[ Min(0f) ]
		public float EstimatedTime = 10f;
		
		public OutlineData Outline;
	}
}
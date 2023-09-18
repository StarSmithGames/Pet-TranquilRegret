using UnityEngine;

namespace Game.Systems.LevelSystem
{
	[System.Serializable]
	public class GoalItem
	{
		public GoalConfig config;
		[Min(1)]
		public int count = 1;
	}
}
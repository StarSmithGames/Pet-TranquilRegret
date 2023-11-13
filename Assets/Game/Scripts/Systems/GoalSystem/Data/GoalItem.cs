using UnityEngine;

namespace Game.Systems.GoalSystem
{
	[System.Serializable]
	public class GoalItem
	{
		public GoalConfig config;
		[Min(1)]
		public int count = 1;
	}
}
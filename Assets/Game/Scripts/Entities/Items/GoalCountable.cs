using Game.Managers.LevelManager;
using Game.Systems.FloatingSystem;
using UnityEngine;

using Zenject;

namespace Game.Entities
{
	public abstract class GoalCountable : Floating3DObject
	{
		public CountableGoalConfig Data => data;
		public int Count => count;

		public GoalBar Goal { get; private set; }

		[SerializeField] private CountableGoalConfig data;
		[Min(1)]
		[SerializeField] private int count = 1;
	}
}
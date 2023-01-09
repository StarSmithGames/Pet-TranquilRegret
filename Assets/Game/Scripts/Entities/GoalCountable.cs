using Game.Managers.LevelManager;
using Game.Systems.FloatingSystem;
using UnityEngine;

using Zenject;

namespace Game.Entities
{
	public abstract class GoalCountable : Floating3DObject
	{
		public CountableGoalData Data => data;
		public int Count => count;

		public GoalBar Goal { get; private set; }

		[SerializeField] private CountableGoalData data;
		[Min(1)]
		[SerializeField] private int count = 1;

		private LevelManager levelManager;

		[Inject]
		private void Construct(LevelManager levelManager)
		{
			this.levelManager = levelManager;
		}

		private void Start()
		{
			Goal = levelManager.CurrentLevel.GetCountableGoal(data);
		}
	}
}
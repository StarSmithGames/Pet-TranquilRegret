using Game.Managers.LevelManager;
using Game.Systems.FloatingSystem;
using UnityEngine;

using Zenject;

namespace Game.Entities
{
	public class Apple : Floating3DObject
	{
		[SerializeField] private CountableGoalData data;
		[Min(1)]
		[SerializeField] private int count;

		private CountableGoal goal;

		private LevelManager levelManager;
		private FloatingSystem floatingSystem;

		[Inject]
		private void Construct(LevelManager levelManager, FloatingSystem floatingSystem)
		{
			this.levelManager = levelManager;
			this.floatingSystem = floatingSystem;
		}

		private void Start()
		{
			goal = levelManager.CurrentLevel.GetCountableGoal(data);
		}

		protected override void OnAnimationCompleted()
		{
			floatingSystem.CreateText(target.position, $"+{count}", data.information.portrait);
			
			goal.CurrentValue += count;
		}
	}
}
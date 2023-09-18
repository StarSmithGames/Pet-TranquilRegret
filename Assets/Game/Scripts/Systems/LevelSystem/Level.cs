using Game.Managers.GameManager;
using Game.Systems.StorageSystem;

using System;
using System.Collections.Generic;
using System.Linq;

using Zenject;

namespace Game.Systems.LevelSystem
{
	public class Level
	{
		[Inject] private GameData gameData;
		[Inject] private GameManager gameManager;
		[Inject] private SpawnSystem.SpawnSystem spawnSystem;


		public GoalRegistrator GoalRegistrator { get; private set; }

		public LevelConfig config;

		public Level(LevelConfig config)
		{
			ProjectContext.Instance.Container.Inject(this);

			this.config = config;
			GoalRegistrator = new GoalRegistrator(config);
		}

		public void Start()
		{
			gameManager.ChangeState(GameState.PreGameplay);
			spawnSystem.SpawnPlayer();
		}

		public void Complete()
		{

		}

		public void Lose()
		{

		}

		public void Leave()
		{

		}
	}

	public class GoalRegistrator
	{
		public event Action onAccumulatedPrimary;

		public List<IGoal> GoalsPrimary { get; private set; }

		public GoalRegistrator(LevelConfig config)
		{
			GoalsPrimary = new List<IGoal>();
			for (int i = 0; i < config.primaryGoals.Count; i++)
			{
				GoalsPrimary.Add(new Goal(config.primaryGoals[i]));
			}
		}

		public void AccumulatePrimaryGoal(GoalItem item)
		{
			var goal = GoalsPrimary.Find((x) => x.Config == item.config);
			goal.CurrentValue += item.count;

			onAccumulatedPrimary?.Invoke();
		}

		public void AccumulatePrimaryGoals(GoalItem[] items)
		{
			for (int i = 0; i < items.Length; i++)
			{
				var goal = GoalsPrimary.Find((x) => x.Config == items[i].config);
				goal.CurrentValue += items[i].count;
			}

			onAccumulatedPrimary?.Invoke();
		}

		public bool IsPrimaryGoalsCompleted()
		{
			return GoalsPrimary.All((x) => x.IsCompleted);
		}
	}
}
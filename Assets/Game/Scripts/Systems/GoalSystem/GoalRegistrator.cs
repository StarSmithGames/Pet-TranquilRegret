using Game.Systems.LevelSystem;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Systems.GoalSystem
{
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
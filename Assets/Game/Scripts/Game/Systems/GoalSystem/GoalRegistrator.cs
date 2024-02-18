using Game.Systems.InventorySystem;
using Game.Systems.LevelSystem;

using ModestTree;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Game.Systems.GoalSystem
{
	public class GoalRegistrator
	{
		public event Action onAccumulatedPrimary;

		public List<IGoal> GoalsPrimary { get; }

		public GoalRegistrator( LevelConfig config )
		{
			GoalsPrimary = new();
			for ( int i = 0; i < config.primaryGoals.Count; i++ )
			{
				GoalsPrimary.Add( new Goal( config.primaryGoals[ i ] ) );
			}
		}

		public void AccumulatePrimaryGoal(GoalItemConfig config)
		{
			Debug.LogError(config.BaseName);

			var goal = GoalsPrimary.Find((x) => x.Model.config == config);
			Assert.IsNotNull(goal, $"[GoalRegistrator] Goal Not Exist for {config.BaseName}");
			goal.CurrentValue++;

			onAccumulatedPrimary?.Invoke();
		}

		public bool IsPrimaryGoalsCompleted()
		{
			return GoalsPrimary.All((x) => x.IsCompleted);
		}
	}
}
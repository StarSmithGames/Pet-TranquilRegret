using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using Zenject;

namespace Game.Managers.LevelManager
{
    public class LevelManager
    {
        public Level CurrentLevel { get; private set; }

		public LevelManager(SignalBus signalBus,
            SceneManager.SceneManager sceneManager,
			Level.Factory levelFactory)
        {
			CurrentLevel = levelFactory.Create(sceneManager.CurrentLevelSettings);
		}
	}

    public class Level
    {
		public double TotalSeconds { get; private set; }
		public double SecondsToSilver { get; private set; }
		public double SecondsToCooper { get; private set; }

		public float PercentToSilver { get; private set; }
		public float PercentToCooper { get; private set; }

		public double CurrentSeconds { get; private set; }
		public float CurrentPercent => (float)(CurrentSeconds / TotalSeconds);
		public LevelReward CurrentReward { get; private set; } = LevelReward.Gold;

		public Coins Coins = new Coins(0);

		public List<GoalBar> PrimaryGoals { get; private set; } = new List<GoalBar>();
		public List<IGoal> SecondaryGoals { get; private set; } = new List<IGoal>();

		private Dictionary<CountableGoalData, GoalBar> primaryDataAndGoal = new Dictionary<CountableGoalData, GoalBar>();

		public LevelSettings Settings { get; private set; }

        public Level(LevelSettings settings)
        {
            this.Settings = settings;

			TotalSeconds = settings.estimateCooperTime.TotalSeconds;
			SecondsToSilver = settings.estimateGoldTime.TotalSeconds;
			SecondsToCooper = settings.estimateSilverTime.TotalSeconds;

			PercentToSilver = (float)(SecondsToSilver / TotalSeconds);
			PercentToCooper = (float)(SecondsToCooper / TotalSeconds);

			for (int i = 0; i < settings.primaryGoals.Count; i++)
			{
				GoalBar goal = new GoalBar(settings.primaryGoals[i], 0, 0, settings.primaryGoals[i].count);
				goal.onChanged += OnGoalChanged;
				PrimaryGoals.Add(goal);

				primaryDataAndGoal.Add(settings.primaryGoals[i], goal);
			}

			//for (int i = 0; i < settings.primaryGoals.Count; i++)
			//{
			//	SecondaryGoals.Add(new CountableGoal(settings.primaryGoals[i], 0, 0, settings.primaryGoals[i].count));
			//}
		}

		public void SetCurrentSeconds(float secs)
		{
			CurrentSeconds = secs;
		}

		public void SetCurrentReward(LevelReward reward)
		{
			CurrentReward = reward;
		}

		public GameTime GetEstimateTime()
		{
			return CurrentReward == LevelReward.Gold ? Settings.estimateGoldTime : CurrentReward == LevelReward.Silver ? Settings.estimateSilverTime : Settings.estimateCooperTime;
		}

		public GoalBar GetCountableGoal(CountableGoalData data)
		{
			primaryDataAndGoal.TryGetValue(data, out GoalBar goal);
			Assert.IsNotNull(goal);
			return goal;
		}

		private void OnGoalChanged()
		{
			if(PrimaryGoals.All((x) => x.IsCompleted))
			{
				Debug.LogError("WIN Game");
			}
		}

        public class Factory : PlaceholderFactory<LevelSettings, Level> { }
    }

	public enum LevelReward
	{
		Platina,
		Gold,
		Silver,
		Cooper,
	}
}
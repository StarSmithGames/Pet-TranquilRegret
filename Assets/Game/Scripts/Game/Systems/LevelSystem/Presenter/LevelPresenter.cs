using Company.Module.Services.DelayedCallService;

using DG.Tweening.Core.Easing;

using Game.Managers.GameManager;
using Game.Managers.PauseManager;
using Game.Managers.RewardManager;
using Game.Services;
using Game.Systems.GameSystem;
using Game.UI;

using UnityEngine;

using Zenject;

namespace Game.Systems.LevelSystem
{
	public partial class LevelPresenter
	{
		public LevelModel Model { get; private set; }
		public LevelTimer LevelTimer { get; }

		[Inject] private StorageSystem.StorageSystem storageSystem;
		[Inject] private SpawnSystem.SpawnSystem spawnSystem;
		[Inject] private ViewService viewService;
		[Inject] private PauseManager pauseManager;
		[Inject] private RewardManager rewardManager;

		public LevelPresenter(LevelConfig config)
		{
			ProjectContext.Instance.Container.Inject(this);
			Model = new(config);
			LevelTimer = new();

			pauseManager.Registrate(LevelTimer);

			Subscribe();
		}

		public void Dispose()
		{
			LevelTimer.Stop();

			pauseManager.UnRegistrate(LevelTimer);
			pauseManager.UnPause();

			Unsubscribe();
		}

		public void Start()
		{
			spawnSystem.SpawnPlayer();
			LevelTimer.Start();
		}

		public float GetProgress01()
		{
			var goals = Model.GoalRegistrator.GoalsPrimary;
			float percents = 0;
			for (int i = 0; i < goals.Count; i++)
			{
				percents += goals[i].PercentValue;
			}

			return percents / goals.Count;
		}

		public float GetProgress()
		{
			return Mathf.Round(GetProgress01() * 100f);
		}

		public void Complete()
		{
			pauseManager.Pause();

			LevelTimer.Stop();

			var data = storageSystem.GamePlayData.Storage.GameProgress.GetData();
			var level = data.regularLevels[storageSystem.GameFastData.LastRegularLevelIndex];
			level.completed = 1;
			level.stars = 3;
			level.timestamp = LevelTimer.Ticks;

			Model.Config.awards.ForEach((award) =>
			{
				rewardManager.Award(award);
			});

			storageSystem.Save();

			viewService.TryShowDialog<FinishLevelDialog>();
		}

		public void Lose()
		{
			Dispose();
			//gameLoader.LoadMenu();
		}

		public void Leave()
		{
			Dispose();
			//gameLoader.LoadMenu();
		}

		private void OnGoalsChanged()
		{
			if (Model.IsCompleted())
			{
				Complete();
			}
		}
	}

	public partial class LevelPresenter
	{
		private void Subscribe()
		{
			Model.GoalRegistrator.onAccumulatedPrimary += OnGoalsChanged;
		}

		private void Unsubscribe()
		{
			Model.GoalRegistrator.onAccumulatedPrimary -= OnGoalsChanged;
		}
	}
}
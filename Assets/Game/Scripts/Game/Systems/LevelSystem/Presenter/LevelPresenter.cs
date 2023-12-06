using Company.Module.Services.DelayedCallService;

using Game.Managers.GameManager;
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

		[Inject] private GameLoader gameLoader;
		[Inject] private GameManager gameManager;
		[Inject] private StorageSystem.StorageSystem storageSystem;
		[Inject] private SpawnSystem.SpawnSystem spawnSystem;
		[Inject] private ViewService viewService;
		[Inject] private IDelayedCallService delayedCallService;

		public LevelPresenter(LevelConfig config)
		{
			ProjectContext.Instance.Container.Inject(this);

			Model = new(config);
			LevelTimer = new();

			Subscribe();
		}

		public void Dispose()
		{
			LevelTimer.Stop();
			Unsubscribe();
		}

		public void Start()
		{
			gameLoader.LoadLevel(Model.Config, false,
			onCompleted: () =>
			{
				gameManager.ChangeState(GameState.PreGameplay);
				spawnSystem.SpawnPlayer();
				gameLoader.Allow();
			},
			callback: () =>
			{
				LevelTimer.Start();
				gameManager.ChangeState(GameState.Gameplay);
			});
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
			LevelTimer.Stop();

			var data = storageSystem.GamePlayData.Storage.GameProgress.GetData();
			var level = data.regularLevels[storageSystem.GameFastData.LastRegularIndex];
			level.completed = 1;
			level.stars = 3;
			level.timestamp = LevelTimer.Ticks;
			storageSystem.Save();

			viewService.TryShowDialog<LevelFinishDialog>();
		}

		public void Lose()
		{
			Dispose();
			gameLoader.LoadMenu();
		}

		public void Leave()
		{
			Dispose();
			gameLoader.LoadMenu();
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
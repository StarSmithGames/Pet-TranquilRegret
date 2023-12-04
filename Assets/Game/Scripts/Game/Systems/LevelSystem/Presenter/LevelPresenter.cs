using Company.Module.Services.DelayedCallService;

using Game.Managers.GameManager;
using Game.Systems.GameSystem;
using Game.Systems.StorageSystem;

using UnityEngine;

using Zenject;

namespace Game.Systems.LevelSystem
{
	public partial class LevelPresenter
	{
		public LevelModel Model { get; private set; }

		[Inject] private GameLoader gameLoader;
		[Inject] private GameData gameData;
		[Inject] private SpawnSystem.SpawnSystem spawnSystem;
		[Inject] private IDelayedCallService delayedCallService;

		public LevelPresenter(LevelConfig config)
		{
			ProjectContext.Instance.Container.Inject(this);

			Model = new(config);

			Subscribe();
		}

		public void Dispose()
		{
			Unsubscribe();
		}

		public void Start()
		{
			spawnSystem.SpawnPlayer();
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
			var data = gameData.Storage.GameProgress.GetData();
			data.progressMainIndex++;

			delayedCallService.DelayedCallAsync(1.69f, () =>
			{
				Dispose();
				gameLoader.LoadMenu();
			});
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
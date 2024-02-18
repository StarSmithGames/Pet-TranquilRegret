using Game.Managers.PauseManager;
using Game.Managers.RewardManager;
using System;
using UnityEngine;

using Zenject;

namespace Game.Systems.LevelSystem
{
	public sealed class LevelPresenter : IDisposable, IPausable
	{
		public LevelModel Model { get; }
		public LevelViewModel ViewModel { get; }
		public LevelGameplay Gameplay { get; }
		public EstimatedTimer Timer { get; }

		[ Inject ] private StorageSystem.StorageSystem storageSystem;
		[ Inject ] private RewardManager rewardManager;

		public LevelPresenter(
			LevelModel model,
			LevelGameplay gameplay,
			EstimatedTimer timer
			)
		{
			Model = model ?? throw new ArgumentNullException( nameof(model) );
			ViewModel = new();
			Gameplay = gameplay ?? throw new ArgumentNullException( nameof(gameplay) );
			Timer = timer ?? throw new ArgumentNullException( nameof(timer) );
			
			ProjectContext.Instance.Container.Inject( this );
		}

		public void Dispose()
		{
			Timer.Stop();
		}

		public void Start()
		{
			Timer.StartEstimatedTimer( Model.Config.estimatedTime );
		}

		public float GetProgress01()
		{
			var goals = Gameplay.GoalRegistrator.GoalsPrimary;
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
			Timer.Stop();

			var data = storageSystem.GamePlayData.Storage.GameProgress.GetData();
			var level = data.regularLevels[storageSystem.GameFastData.LastRegularLevelIndex];
			level.completed = 1;
			level.stars = 3;
			level.timestamp = Timer.RemainingTime;

			Model.Config.awards.ForEach((award) =>
			{
				rewardManager.Award(award);
			});

			storageSystem.Save();

			// viewService.TryShowDialog<FinishLevelDialog>();
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

		public void Pause()
		{
			Timer.Pause();
		}

		public void UnPause()
		{
			Timer.UnPause();
		}
	}
}
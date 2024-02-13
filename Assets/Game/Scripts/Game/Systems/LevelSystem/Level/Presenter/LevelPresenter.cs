using Game.Managers.PauseManager;
using Game.Managers.RewardManager;
using Game.Services;
using Game.UI;
using System;
using UnityEngine;

using Zenject;

namespace Game.Systems.LevelSystem
{
	public class LevelPresenter : IDisposable, IPausable
	{
		public LevelModel Model { get; }
		public LevelViewModel ViewModel { get; }
		public LevelGameplay Gameplay { get; }
		public LevelTimer LevelTimer { get; }

		[Inject] private StorageSystem.StorageSystem storageSystem;
		[Inject] private SpawnSystem.SpawnSystem spawnSystem;
		[Inject] private ViewService viewService;
		[Inject] private RewardManager rewardManager;

		public LevelPresenter(
			LevelModel model,
			LevelGameplay gameplay
			)
		{
			Model = model ?? throw new ArgumentNullException( nameof(model) );
			ViewModel = new();
			Gameplay = gameplay ?? throw new ArgumentNullException( nameof(gameplay) );
			LevelTimer = new();
			
			ProjectContext.Instance.Container.Inject( this );
		}

		public void Dispose()
		{
			LevelTimer.Stop();
		}

		public void Start()
		{
			spawnSystem.SpawnPlayer();
			LevelTimer.Start();
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

		public void Pause()
		{
			LevelTimer.Pause();
		}

		public void UnPause()
		{
			LevelTimer.UnPause();
		}
	}
}
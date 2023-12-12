using Game.Extensions;
using Game.Managers.GameManager;
using Game.Systems.GameSystem;

using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using Zenject;

namespace Game.Systems.LevelSystem
{
	public class LevelManager
	{
		public event Action OnLevelStarted;
		public event Action OnLevelLeaved;

		public LevelPresenter CurrentLevel { get; private set; }

		[Inject] private GameplayConfig gameplayConfig;
		[Inject] private GameLoader gameLoader;
		[Inject] private GameManager gameManager;

		public void StartRegularLevel(LevelConfig levelConfig)
        {
			CurrentLevel?.Dispose();

			gameLoader.LoadLevel(levelConfig, true,
			onShowed: () =>
			{
				CurrentLevel = new(levelConfig);
			},
			onCompleted: () =>
			{
				CurrentLevel.Start();
				gameManager.ChangeState(GameState.PreGameplay);
			},
			callback: () =>
			{
				gameManager.ChangeState(GameState.Gameplay);
			});

			OnLevelStarted?.Invoke();
		}

        public void LeaveLevel()
        {
			CurrentLevel.Leave();
			CurrentLevel = null;

			OnLevelLeaved?.Invoke();
		}

		public LevelConfig GetLevelConfig(int number)
		{
			return gameplayConfig.levels[number - 1];
		}
	}
}
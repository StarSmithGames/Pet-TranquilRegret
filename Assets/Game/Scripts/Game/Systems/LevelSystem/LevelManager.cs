
using DG.Tweening.Core.Easing;

using Game.Extensions;
using Game.Managers.GameManager;
using Game.Managers.TransitionManager;
using Game.Systems.GameSystem;

using StarSmithGames.Go.SceneManager;
using StarSmithGames.IoC.AsyncManager;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

using Zenject;

namespace Game.Systems.LevelSystem
{
    public class LevelManager : IInitializable
	{
		public event Action OnLevelStarted;
		public event Action OnLevelLeaved;

		public LevelPresenter CurrentLevel { get; private set; }

		[Inject] private StarSmithGames.Go.SceneManager.SceneManager sceneManager;
		[Inject] private GameplayConfig gameplayConfig;

		public void Initialize()
		{
#if UNITY_EDITOR
			if (sceneManager.IsLevel())
			{
				var config = GetLevelConfigFull(sceneManager.GetActiveScene());
				CurrentLevel = new(config);
				CurrentLevel.Start();
			}
#endif
		}

		public void StartRegularLevel(LevelConfig levelConfig)
        {
			CurrentLevel?.Dispose();
			CurrentLevel = new(levelConfig);
			CurrentLevel.Start();

			OnLevelStarted?.Invoke();
		}

        public void LeaveLevel()
        {
			CurrentLevel.Leave();
			CurrentLevel = null;

			OnLevelLeaved?.Invoke();
		}

		public LevelConfig GetLevelConfigFull(Scene scene)
		{
			return gameplayConfig.GetAllLevels().Find((x) =>
			{
				Debug.LogError(x.scene.SceneName + " " + scene.name);

				return x.scene.SceneName == scene.name;
			});
		}

		public LevelConfig GetLevelConfig(int number)
		{
			return gameplayConfig.levels[number - 1];
		}
	}
}
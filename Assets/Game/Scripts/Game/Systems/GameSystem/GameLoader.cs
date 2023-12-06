using Game.Extensions;
using Game.Managers.GameManager;
using Game.Managers.TransitionManager;
using Game.Systems.LevelSystem;
using Game.Systems.StorageSystem;

using StarSmithGames.Go.SceneManager;
using StarSmithGames.IoC.AsyncManager;

using System;
using System.Collections;
using System.IO;

using UnityEngine;
using UnityEngine.Assertions;

using Zenject;

namespace Game.Systems.GameSystem
{
	public class GameLoader
	{
		[Inject] private GameManager gameManager;
		[Inject] private SceneManager sceneManager;
		[Inject] private TransitionManager transitionManager;

		private Transition startLevelTransition;

		public void LoadMenu(Action onShowed = null, Action onCompleted = null, Action onHided = null, Action callback = null)
		{
			gameManager.ChangeState(GameState.Loading);

			var transition = new Transition(
			() =>
			{
				sceneManager.LoadSceneAsyncFromBuild(1, true);//Menu
				return sceneManager.ProgressHandler;
			}, true, onCompleted);
			transitionManager.StartInfinityLoading(transition, onShowed,
			onHided: () =>
			{
				gameManager.ChangeState(GameState.Menu);
				onHided?.Invoke();
			}, callback);
		}

		public void LoadLevel(LevelConfig levelConfig, bool allow, Action onShowed = null, Action onCompleted = null, Action onHided = null, Action callback = null)
		{
			gameManager.ChangeState(GameState.Loading);
			startLevelTransition = new Transition(
			() =>
			{
				var name = levelConfig.scene.SceneName;
				sceneManager.LoadSceneAsyncFromAddressables(name, name);
				return sceneManager.ProgressHandler;
			}, allow, onCompleted);
			transitionManager.StartInfinityLoading(startLevelTransition, onShowed, onHided, callback);
		}

		public void Allow()
		{
			startLevelTransition?.Allow();
			startLevelTransition = null;
		}
	}
}
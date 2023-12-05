using Game.Extensions;
using Game.Managers.GameManager;
using Game.Managers.TransitionManager;
using Game.Systems.LevelSystem;
using Game.Systems.StorageSystem;

using StarSmithGames.Go.SceneManager;
using StarSmithGames.IoC.AsyncManager;

using System.Collections;
using System.IO;

using UnityEngine;
using UnityEngine.Assertions;

using Zenject;

namespace Game.Systems.GameSystem
{
	public class GameLoader : IInitializable
	{
		[Inject] private AsyncManager asyncManager;
		[Inject] private GameManager gameManager;
		[Inject] private StorageSystem.StorageSystem gameData;
		[Inject] private SceneManager sceneManager;
		[Inject] private TransitionManager transitionManager;

		private Transition startLevelTransition;

		public void Initialize()
		{
#if UNITY_EDITOR
			if (sceneManager.IsLevel())
			{
				asyncManager.StartCoroutine(GamePipeline());
			}
#endif
		}

		public void LoadMenu()
		{
			gameData.IntermediateData.LevelPresenter = null;

			gameManager.ChangeState(GameState.Loading);

			var transition = new Transition(
			() =>
			{
				sceneManager.LoadSceneAsyncFromBuild(1, true);//Menu
				return sceneManager.ProgressHandler;
			});
			transitionManager.StartInfinityLoading(transition, onHided: () => gameManager.ChangeState(GameState.Menu));
		}

		public void LoadLevel(LevelConfig levelConfig)
		{
			gameManager.ChangeState(GameState.Loading);
			startLevelTransition = new Transition(
			() =>
			{
				var name = levelConfig.scene.SceneName;
				sceneManager.LoadSceneAsyncFromAddressables(name, name);
				return sceneManager.ProgressHandler;
			}, false,
			() =>
			{
				asyncManager.StartCoroutine(GamePipeline());
			});
			transitionManager.StartInfinityLoading(startLevelTransition, onShowed: () =>
			{
				gameData.IntermediateData.LevelPresenter = new(levelConfig);
			});
		}

		private IEnumerator GamePipeline()
		{
#if UNITY_EDITOR
			if (gameData.IntermediateData.LevelPresenter == null)
			{
				var config = gameData.IntermediateData.GetLevelConfig(sceneManager.GetActiveScene());
				Assert.IsNotNull(config);
				gameData.IntermediateData.LevelPresenter = new(config);
			}
#endif
			gameManager.ChangeState(GameState.PreGameplay);
			gameData.IntermediateData.LevelPresenter.Start();
			OnDragChanged();

			yield return null;
			startLevelTransition?.Allow();
			startLevelTransition = null;
		}

		private void OnDragChanged()
		{
			if (gameManager.CurrentGameState != GameState.Gameplay)
			{
				gameManager.ChangeState(GameState.Gameplay);
			}
		}
	}
}
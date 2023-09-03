using Game.Extensions;
using Game.Managers.GameManager;
using Game.Managers.TransitionManager;
using Game.Systems.SpawnSystem;

using StarSmithGames.Go.SceneManager;
using StarSmithGames.IoC.AsyncManager;

using System.Collections;
using System.IO;

using UnityEngine;

using Zenject;

namespace Game.Systems.GameSystem
{
	public class GameLoader : IInitializable
	{
		[Inject] private AsyncManager asyncManager;
		[Inject] private GameManager gameManager;
		[Inject] private SceneManager sceneManager;
		[Inject] private TransitionManager transitionManager;
		[Inject] private SpawnSystem.SpawnSystem spawnSystem;

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
				var name = Path.GetFileNameWithoutExtension(levelConfig.scene.ScenePath);
				sceneManager.LoadSceneAsyncFromAddressables(name, name);
				return sceneManager.ProgressHandler;
			}, false,
			() =>
			{
				asyncManager.StartCoroutine(GamePipeline());
			});
			transitionManager.StartInfinityLoading(startLevelTransition, onHided: () => Debug.LogError("Load Completed!") );
		}

		private IEnumerator GamePipeline()
		{
			gameManager.ChangeState(GameState.PreGameplay);
			spawnSystem.SpawnPlayer();

			yield return null;
			startLevelTransition?.Allow();
			startLevelTransition = null;
		}
	}
}
using Game.Managers.GameManager;
using Game.Managers.TransitionManager;

using StarSmithGames.Go.SceneManager;

using System.IO;

using UnityEngine;

using Zenject;

namespace Game.Systems.GameSystem
{
	public class GameLoader
	{
		[Inject] private GameManager gameManager;
		[Inject] private SceneManager sceneManager;
		[Inject] private TransitionManager transitionManager;

		public void LoadMenu()
		{
			gameManager.ChangeState(GameState.Loading);
			transitionManager.StartInfinityLoading(() =>
			{
				sceneManager.LoadSceneAsyncFromBuild(1, true);
				return sceneManager.ProgressHandler;
			}, onHided: () => gameManager.ChangeState(GameState.Menu));
		}

		public void LoadLevel(LevelConfig levelConfig)
		{
			gameManager.ChangeState(GameState.Loading);
			transitionManager.StartInfinityLoading(
			() =>
			{
				var name = Path.GetFileNameWithoutExtension(levelConfig.scene.ScenePath);
				sceneManager.LoadSceneAsyncFromAddressables(name, name);
				return sceneManager.ProgressHandler;
			}, onHided: () => gameManager.ChangeState(GameState.PreGameplay));
		}
	}
}
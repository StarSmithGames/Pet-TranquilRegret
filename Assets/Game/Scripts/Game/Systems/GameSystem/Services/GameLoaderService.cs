using Game.Managers.GameManager;
using Game.Managers.TransitionManager;
using StarSmithGames.Go.SceneManager;
using System;

namespace Game.Systems.GameSystem
{
	public sealed class GameLoaderService
	{
		private Transition _startLevelTransition;
		
		private GameManager _gameManager;
		private SceneManager _sceneManager;
		private TransitionManager _transitionManager;

		public GameLoaderService(
			GameManager gameManager,
			SceneManager sceneManager,
			TransitionManager transitionManager
			)
		{
			_gameManager = gameManager;
			_sceneManager = sceneManager;
			_transitionManager = transitionManager;
		}

		public void LoadMenu(Action onShowed = null, Action onCompleted = null, Action onHided = null, Action callback = null)
		{
			_gameManager.ChangeState(GameState.Loading);

			var transition = new Transition(
			() =>
			{
				_sceneManager.LoadSceneAsyncFromBuild(1, true);//Menu
				return _sceneManager.ProgressHandler;
			}, true, onCompleted);
			_transitionManager.StartInfinityLoadingAsync(transition, onShowed,
			onHided: () =>
			{
				_gameManager.ChangeState(GameState.Menu);
				onHided?.Invoke();
			}, callback);
		}

		public void LoadLevel( string sceneName, bool allow, Action onShowed = null, Action onCompleted = null, Action onHided = null, Action callback = null )
		{
			_gameManager.ChangeState(GameState.Loading);

			_startLevelTransition = new Transition(
			() =>
			{
				var name = sceneName;
				_sceneManager.LoadSceneAsyncFromAddressables(name, name);
				return _sceneManager.ProgressHandler;
			}, allow, onCompleted);
			_transitionManager.StartInfinityLoadingAsync(_startLevelTransition, onShowed, onHided, callback);
		}

		public void Allow()
		{
			_startLevelTransition?.Allow();
			_startLevelTransition = null;
		}
	}
}
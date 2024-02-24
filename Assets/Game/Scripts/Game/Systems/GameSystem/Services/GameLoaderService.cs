using Cysharp.Threading.Tasks;
using Game.Managers.AssetManager;
using Game.Managers.GameManager;
using Game.Managers.TransitionManager;
using System;

namespace Game.Systems.GameSystem
{
	public sealed class GameLoaderService
	{
		private Transition _startLevelTransition;
		
		private GameManager _gameManager;
		private SceneSystem.SceneSystem _sceneSystem;
		private ResourcesManager _resourcesManager;
		private TransitionManager _transitionManager;

		public GameLoaderService(
			GameManager gameManager,
			SceneSystem.SceneSystem sceneSystem,
			ResourcesManager resourcesManager,
			TransitionManager transitionManager
			)
		{
			_gameManager = gameManager;
			_sceneSystem = sceneSystem;
			_resourcesManager = resourcesManager;
			_transitionManager = transitionManager;
		}

		public void LoadMenu(Action onShowed = null, Action onCompleted = null, Action onHided = null, Action callback = null)
		{
			_gameManager.ChangeState(GameState.Loading);

			var transition = new Transition(
			() =>
			{
				_sceneSystem.LoadSceneAsyncFromBuild(1, true).Forget();//Menu
				return _sceneSystem.ProgressHandler;
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
				_sceneSystem.LoadSceneFromAddressables(name, name, allow).Forget();
				return _sceneSystem.ProgressHandler;
			}, allow, () =>
			{
				_resourcesManager.UpdateResourcesMaterials();
				onCompleted?.Invoke();
			} );
			_transitionManager.StartInfinityLoadingAsync(_startLevelTransition, onShowed, onHided, callback);
		}

		public void Allow()
		{
			_startLevelTransition?.Allow();
			_startLevelTransition = null;
		}
	}
}
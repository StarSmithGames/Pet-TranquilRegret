using Game.Systems.GameSystem;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Systems.LevelSystem
{
	public sealed class LevelManager
	{
		public event Action< ILevel > OnLevelBuilded;
		public event Action< ILevel > OnLevelStarted;
		public event Action< ILevel > OnLevelDisposed;

		public ILevel CurrentLevel { get; private set; }

		private readonly GameLoaderService _gameLoaderService;

		public LevelManager(
			GameLoaderService gameLoaderService
			)
		{
			_gameLoaderService = gameLoaderService ?? throw new ArgumentNullException( nameof(gameLoaderService) );
		}

		public void StartLevel( string sceneName, ILevelBuilder builder, Action onBuilded = null, Action onStarted = null, Action callback = null )
        {
			CurrentLevel?.Dispose();

			_gameLoaderService.LoadLevel( sceneName, true,
			onCompleted: () =>
			{
				BuildLevel( builder );
				onBuilded?.Invoke();
			},
			callback:()=>
			{
				CurrentLevel?.Start();
				OnLevelStarted?.Invoke( CurrentLevel );
				
				callback?.Invoke();
			} );
		}

        public void LeaveLevel( Action onDisposed = null, Action callback = null )
        {
			_gameLoaderService.LoadMenu(
			onShowed: () =>
			{
				DisposeLevel();
				onDisposed?.Invoke();
			},
			callback: callback );
		}

        private void BuildLevel( ILevelBuilder builder )
        {
	        CurrentLevel = builder.Build();
			OnLevelBuilded?.Invoke( CurrentLevel );
        }

        private void DisposeLevel()
        {
	        CurrentLevel.Stop();
	        OnLevelDisposed?.Invoke( CurrentLevel );
	        CurrentLevel = null;
        }
	}
}
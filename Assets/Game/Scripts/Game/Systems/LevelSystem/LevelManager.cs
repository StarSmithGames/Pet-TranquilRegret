using Game.Systems.GameSystem;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Systems.LevelSystem
{
	public sealed class LevelManager
	{
		public event Action OnLevelBuilded;
		public event Action OnLevelStarted;
		public event Action OnLevelDisposed;

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
				OnLevelStarted?.Invoke();
				
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
			OnLevelBuilded?.Invoke();
        }

        private void DisposeLevel()
        {
	        CurrentLevel.Stop();
	        OnLevelDisposed?.Invoke();
	        CurrentLevel = null;
        }
	}
}
using Game.Systems.GameSystem;
using System;

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
			onShowed: () =>
			{
				CurrentLevel = builder.Build();
				onBuilded?.Invoke();
				OnLevelBuilded?.Invoke( CurrentLevel );
			},
			onCompleted: () =>
			{
				CurrentLevel?.Start();
				onStarted?.Invoke();
				OnLevelStarted?.Invoke( CurrentLevel );
			},
			callback: callback );
		}

        public void LeaveLevel( Action onDisposed = null, Action callback = null )
        {
			_gameLoaderService.LoadMenu(
			onShowed: () =>
			{
				CurrentLevel.Stop();
				onDisposed?.Invoke();
				OnLevelDisposed?.Invoke( CurrentLevel );
				CurrentLevel = null;
			},
			callback: callback );
		}
	}
}
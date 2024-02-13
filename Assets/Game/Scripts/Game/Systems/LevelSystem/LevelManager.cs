using Game.Systems.GameSystem;
using System;

namespace Game.Systems.LevelSystem
{
	public sealed class LevelManager
	{
		public ILevel CurrentLevel { get; private set; }

		private readonly GameLoader _gameLoader;

		public LevelManager(
			GameLoader gameLoader
			)
		{
			_gameLoader = gameLoader ?? throw new ArgumentNullException( nameof(gameLoader) );
		}

		public void StartLevel( string sceneName, ILevelBuilder builder, Action onBuilded = null, Action onStarted = null, Action callback = null )
        {
			CurrentLevel?.Dispose();

			_gameLoader.LoadLevel( sceneName, true,
			onShowed: () =>
			{
				CurrentLevel = builder.Build();
				onBuilded?.Invoke();
			},
			onCompleted: () =>
			{
				CurrentLevel.Start();
				onStarted?.Invoke();
			},
			callback: callback );
		}

        public void LeaveLevel( Action onDisposed = null, Action callback = null )
        {
			_gameLoader.LoadMenu(
			onShowed: () =>
			{
				CurrentLevel.Stop();
				CurrentLevel = null;
				onDisposed?.Invoke();
			},
			callback: callback );
		}
	}
}
using System;

namespace Game.Systems.LevelSystem
{
	public sealed class RegularLevel : ILevel
	{
		public event Action OnStarted;
		public event Action OnCompleted
		{
			add => Presenter.Gameplay.OnCompleted += value;
			remove => Presenter.Gameplay.OnCompleted -= value;
		}
		public event Action OnDisposed;
		
		public LevelPresenter Presenter { get; }

		public RegularLevel(
			LevelPresenter presenter
			)
		{
			Presenter = presenter ?? throw new ArgumentNullException( nameof(presenter) );
		}

		public void Dispose()
		{
			Presenter?.Dispose();
			
			OnDisposed?.Invoke();
		}

		public void Start()
		{
			Presenter.Start();
			
			OnStarted?.Invoke();
		}

		public void Stop()
		{
			Dispose();
		}
	}
}
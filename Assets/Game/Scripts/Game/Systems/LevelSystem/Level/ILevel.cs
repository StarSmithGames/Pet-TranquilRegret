using System;

namespace Game.Systems.LevelSystem
{
	public interface ILevel : IDisposable
	{
		event Action OnStarted;
		event Action OnCompleted;
		event Action OnDisposed;
		
		LevelPresenter Presenter { get; }
		LevelViewModel ViewModel { get; }
		
		void Start();
		void Complete();
		void Stop();
	}
}
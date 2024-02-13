using System;

namespace Game.Systems.LevelSystem
{
	public interface ILevel : IDisposable
	{
		event Action OnStarted;
		event Action OnCompleted;
		event Action OnDisposed;
		
		public LevelPresenter Presenter { get; }
		
		public void Start();
		public void Stop();
	}
}
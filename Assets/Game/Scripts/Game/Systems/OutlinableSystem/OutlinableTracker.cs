using Game.Systems.LevelSystem;
using System;
using Zenject;

namespace Game.Services
{
	public sealed class OutlinableTracker : IInitializable, IDisposable
	{
		private readonly OutlinableManager _outlinableManager;
		private readonly LevelManager _levelManager;

		public OutlinableTracker(
			OutlinableManager outlinableManager,
			LevelManager levelManager
			)
		{
			_outlinableManager = outlinableManager ?? throw new ArgumentNullException( nameof(outlinableManager) );
			_levelManager = levelManager ?? throw new ArgumentNullException( nameof(levelManager) );
		}

		public void Initialize()
		{
			_levelManager.OnLevelBuilded += LevelBuilded;
			_levelManager.OnLevelDisposed += LevelDisposed;
		}

		public void Dispose()
		{
			_levelManager.OnLevelBuilded -= LevelBuilded;
			_levelManager.OnLevelDisposed -= LevelDisposed;
		}

		private void LevelBuilded()
		{
			var outlinables = _levelManager.CurrentLevel.ViewModel.View.GetOutlinables();
			
			_outlinableManager.AddObservers( outlinables );
		}

		private void LevelDisposed()
		{
			var outlinables = _levelManager.CurrentLevel.ViewModel.View.GetOutlinables();
			
			_outlinableManager.RemoveObservers( outlinables );
		}
	}
}
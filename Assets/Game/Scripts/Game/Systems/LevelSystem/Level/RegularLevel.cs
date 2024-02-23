using System;
using UnityEngine;

namespace Game.Systems.LevelSystem
{
	public sealed class RegularLevel : ILevel
	{
		public event Action OnStarted;
		public event Action OnCompleted;
		public event Action OnDisposed;
		
		public LevelPresenter Presenter { get; }
		public LevelRegularViewModel ViewModel { get; }

		public RegularLevel(
			LevelPresenter presenter,
			LevelRegularViewModel viewModel
			)
		{
			Presenter = presenter ?? throw new ArgumentNullException( nameof(presenter) );
			ViewModel = viewModel ?? throw new ArgumentNullException( nameof(viewModel) );

			Presenter.Gameplay.OnCompleted += GameplayCompletedHandler;
		}

		public void Dispose()
		{
			if ( Presenter != null )
			{
				Presenter.Gameplay.OnCompleted -= GameplayCompletedHandler;
				Presenter.Dispose();
			}
			
			OnDisposed?.Invoke();
		}

		public void Start()
		{
			Presenter.Start();
			ViewModel.Start();
			
			Debug.LogError( "STArt" );
			
			OnStarted?.Invoke();
		}
		
		public void Complete()
		{
			Presenter.Complete();
			ViewModel.Complete();
			
			OnCompleted?.Invoke();
		}

		public void Stop()
		{
			Dispose();
		}

		private void GameplayCompletedHandler()
		{
			Complete();
		}
	}
}
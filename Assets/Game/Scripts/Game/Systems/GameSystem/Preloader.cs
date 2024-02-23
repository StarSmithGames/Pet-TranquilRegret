using Game.Systems.UISystem;
using Game.UI;
using System;

namespace Game.Systems.GameSystem
{
	public sealed class Preloader : IDisposable
    {
		private GDPRDialog dialog;

		private readonly UIRootPreloader _uiRootPreloader;
		private readonly StorageSystem.StorageSystem _storageSystem;
		private readonly GameplayConfig _gameplayConfig;
		private readonly GameLoaderService _gameLoaderService;
		private readonly GameService _gameService;
		
		public Preloader(
			UIRootPreloader uiRootPreloader,
			StorageSystem.StorageSystem storageSystem,
			GameplayConfig gameplayConfig,
			GameLoaderService gameLoaderService,
			GameService gameService
			)
		{
			_uiRootPreloader = uiRootPreloader;
			_storageSystem = storageSystem;
			_gameplayConfig = gameplayConfig;
			_gameLoaderService = gameLoaderService;
			_gameService = gameService;
			
			Initialize();
		}
		
		private void Initialize()
		{
			_storageSystem.GameFastData.SessionsCount++;

			if (!_storageSystem.GameFastData.IsGDPRApplied)
			{
				dialog = _uiRootPreloader.DialogAggregator.CreateIfNotExist< GDPRDialog >();
				dialog.onAgreeClicked += OnAgreeClicked;
				dialog.Show();
			}
			else
			{
				LoadGame();
			}
		}

		public void Dispose()
		{
			if(dialog != null)
			{
				dialog.onAgreeClicked -= OnAgreeClicked;
			}
		}


		private void LoadGame()
		{
			if (_storageSystem.GameFastData.IsFirstTime)
			{
				_gameService.StartRegularLevel( _gameplayConfig.firstTimeTutorialLevel );
			}
			else
			{
				_gameLoaderService.LoadMenu();
			}
		}

		private void OnAgreeClicked()
		{
			_storageSystem.GameFastData.IsGDPRApplied = true;

			if (dialog != null)
			{
				dialog.onAgreeClicked -= OnAgreeClicked;
				dialog.Hide(() =>
				{
					LoadGame();
				});
			}
			else
			{
				LoadGame();
			}
		}
    }
}
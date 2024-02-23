using Game.Systems.UISystem;
using Game.UI;
using System;

namespace Game.Systems.GameSystem
{
	public sealed class Preloader : IDisposable
    {
		private GdprDialog _dialog;

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
				_dialog = _uiRootPreloader.DialogAggregator.CreateIfNotExist< GdprDialog >();
				_dialog.onAgreeClicked += OnAgreeClicked;
				_dialog.Show();
			}
			else
			{
				LoadGame();
			}
		}

		public void Dispose()
		{
			if(_dialog != null)
			{
				_dialog.onAgreeClicked -= OnAgreeClicked;
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

			if (_dialog != null)
			{
				_dialog.onAgreeClicked -= OnAgreeClicked;
				_dialog.Hide(() =>
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
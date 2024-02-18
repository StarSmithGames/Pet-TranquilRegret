using Game.Extensions;
using Game.Systems.LevelSystem;
using StarSmithGames.Go.LocalizationSystem;
using StarSmithGames.Go.SceneManager;
using UnityEngine;
using Zenject;

namespace Game.Systems.GameSystem
{
	public sealed class GamePipeline : IInitializable
	{
		private readonly StorageSystem.StorageSystem _storageSystem;
		private readonly LocalizationSystem _localizationSystem;
		private readonly GameService _gameService;
		private readonly SceneManager _sceneManager;
		private readonly LevelRegularService _levelRegularService;

		private GamePipeline(
			StorageSystem.StorageSystem storageSystem,
			LocalizationSystem localizationSystem,
			GameService gameService,
			SceneManager sceneManager,
			LevelRegularService levelRegularService
			)
		{
			_storageSystem = storageSystem;
			_localizationSystem = localizationSystem;
			_gameService = gameService;
			_sceneManager = sceneManager;
			_levelRegularService = levelRegularService;
		}

		public void Initialize()
		{
			if (_storageSystem.GameFastData.IsFirstTime)
			{
				_storageSystem.GameFastData.LanguageIndex = _localizationSystem.AvailableLocales.IndexOf(_localizationSystem.CurrentLocale);
			}
			else
			{
				_localizationSystem.ChangeLocale(_storageSystem.GameFastData.LanguageIndex);
			}
			
#if UNITY_EDITOR
			if ( _sceneManager.IsLevel() )
			{
				var config = _levelRegularService.GetLevelConfig( _sceneManager.GetActiveScene() );
				_gameService.StartRegularLevel( config );
			}
#endif
		}
	}
}
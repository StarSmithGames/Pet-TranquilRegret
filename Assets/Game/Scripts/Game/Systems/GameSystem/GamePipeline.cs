using Game.Extensions;
using Game.Managers.AssetManager;
using Game.Systems.LevelSystem;
using StarSmithGames.Go.LocalizationSystem;
using Zenject;

namespace Game.Systems.GameSystem
{
	public sealed class GamePipeline : IInitializable
	{
		private readonly StorageSystem.StorageSystem _storageSystem;
		private readonly LocalizationSystem _localizationSystem;
		private readonly GameService _gameService;
		private readonly SceneSystem.SceneSystem _sceneSystem;
		private readonly LevelRegularService _levelRegularService;

		private GamePipeline(
			StorageSystem.StorageSystem storageSystem,
			LocalizationSystem localizationSystem,
			GameService gameService,
			SceneSystem.SceneSystem sceneSystem,
			LevelRegularService levelRegularService
			)
		{
			_storageSystem = storageSystem;
			_localizationSystem = localizationSystem;
			_gameService = gameService;
			_sceneSystem = sceneSystem;
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
			if ( _sceneSystem.IsLevel() )
			{
				var config = _levelRegularService.GetLevelConfig( _sceneSystem.GetActiveScene() );
				_gameService.StartRegularLevel( config );
			}
#endif
		}
	}
}
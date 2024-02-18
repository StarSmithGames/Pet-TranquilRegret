using Game.Systems.GameSystem;
using System;
using UnityEngine.SceneManagement;

namespace Game.Systems.LevelSystem
{
	public sealed class LevelRegularService
	{
		private readonly GameplayConfig _gameplayConfig;

		public LevelRegularService(
			GameplayConfig gameplayConfig
			)
		{
			_gameplayConfig = gameplayConfig ?? throw new ArgumentNullException( nameof(gameplayConfig) );
		}

		public int GetLevelNumber( LevelConfig levelConfig)
		{
			return _gameplayConfig.levels.IndexOf( levelConfig ) + 1;
		}
		
		public LevelConfig GetLevelConfig( int number )
		{
			return _gameplayConfig.levels[ number - 1 ];
		}
		
		public LevelConfig GetLevelConfig( Scene scene )
		{
			return _gameplayConfig.GetAllLevels().Find((x) => x.scene.SceneName == scene.name);
		}
	}
}
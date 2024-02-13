using Game.Systems.GameSystem;
using System;

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
	}
}
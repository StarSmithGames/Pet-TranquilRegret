using System;

namespace Game.Systems.LevelSystem
{
	public sealed class RegularLevelBuilder : ILevelBuilder
	{
		private readonly LevelConfig _levelConfig;
		private readonly LevelRegularService _levelRegularService;
		
		public RegularLevelBuilder(
			LevelConfig levelConfig,
			LevelRegularService levelRegularService
			)
		{
			_levelConfig = levelConfig ?? throw new ArgumentNullException( nameof(levelConfig) );
			_levelRegularService = levelRegularService ?? throw new ArgumentNullException( nameof(levelRegularService) );
		}
		
		public ILevel Build()
		{
			LevelModel model = new( _levelRegularService.GetLevelNumber( _levelConfig ), _levelConfig );
			LevelGameplay gameplay = new( _levelConfig );
			return new RegularLevel( new( model, gameplay ) );
		}
	}
}
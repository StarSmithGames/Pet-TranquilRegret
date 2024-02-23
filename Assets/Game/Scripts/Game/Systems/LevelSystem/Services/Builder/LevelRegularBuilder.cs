using Game.Managers.DIManager;
using System;

namespace Game.Systems.LevelSystem
{
	public sealed class LevelRegularBuilder : ILevelBuilder
	{
		private LevelConfig _config;

		private readonly DIManager _diManager;
		private readonly LevelRegularService _levelRegularService;
		
		public LevelRegularBuilder(
			DIManager diManager,
			LevelRegularService levelRegularService
			)
		{
			_diManager = diManager;
			_levelRegularService = levelRegularService ?? throw new ArgumentNullException( nameof(levelRegularService) );
		}
		
		public void SetConfig( LevelConfig config )
		{
			_config = config;
		}

		public ILevel Build()
		{
			LevelModel model = new( _levelRegularService.GetLevelNumber( _config ), _config );
			LevelGameplay gameplay = new( _config );
			EstimatedTimer timer = new();

			LevelPresenter presenter = new( model, gameplay, timer );
			LevelRegularViewModel viewModel = _diManager.CurrentContainer.Instantiate< LevelRegularViewModel >();
			viewModel.SetPresenter( presenter );
			
			return new RegularLevel( presenter, viewModel );
		}
	}
}
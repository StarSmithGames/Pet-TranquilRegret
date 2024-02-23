using System;
using Zenject;

namespace Game.Systems.LevelSystem
{
	public sealed class LevelRegularBuilder : ILevelBuilder
	{
		private LevelConfig _config;

		private readonly DiContainer _diContainer;
		private readonly LevelRegularService _levelRegularService;
		
		public LevelRegularBuilder(
			DiContainer diContainer,
			LevelRegularService levelRegularService
			)
		{
			_diContainer = diContainer;
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
			LevelRegularViewModel viewModel = _diContainer.Instantiate< LevelRegularViewModel >();
			viewModel.SetPresenter( presenter );
			
			return new RegularLevel( presenter, viewModel );
		}
	}
}
using Game.Managers.GameManager;
using Game.Systems.LevelSystem;
using System;

namespace Game.Services.GameService
{
	public sealed class GameService
	{
		private readonly LevelRegularService _levelRegularService;
		private readonly LevelManager _levelManager;
		private readonly GameManager _gameManager;

		public GameService(
			LevelRegularService levelRegularService,
			LevelManager levelManager,
			GameManager gameManager
			)
		{
			_levelRegularService = levelRegularService ?? throw new ArgumentNullException( nameof(levelRegularService) );
			_levelManager = levelManager ?? throw new ArgumentNullException( nameof(levelManager) );
			_gameManager = gameManager ?? throw new ArgumentNullException( nameof(gameManager) );
		}
		
		public void StartRegularLevel( LevelConfig levelConfig )
		{
			var builder = new RegularLevelBuilder( levelConfig, _levelRegularService );
			_levelManager.StartLevel( levelConfig.scene.SceneName, builder,
			onStarted: () =>
			{
				_gameManager.ChangeState(GameState.PreGameplay);
			},
			callback: () =>
			{
				_gameManager.ChangeState(GameState.Gameplay);
			});
		}

		public void LeaveLevel()
		{
			_levelManager.LeaveLevel(  );
		}
	}
}
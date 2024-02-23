using Game.Managers.GameManager;
using Game.Systems.LevelSystem;
using System;

namespace Game.Systems.GameSystem
{
	public sealed class GameService
	{
		private readonly LevelRegularService _levelRegularService;
		private readonly LevelRegularBuilder _levelRegularBuilder;
		private readonly LevelManager _levelManager;
		private readonly GameManager _gameManager;

		public GameService(
			LevelRegularService levelRegularService,
			LevelRegularBuilder levelRegularBuilder,
			LevelManager levelManager,
			GameManager gameManager
			)
		{
			_levelRegularService = levelRegularService ?? throw new ArgumentNullException( nameof(levelRegularService) );
			_levelRegularBuilder = levelRegularBuilder ?? throw new ArgumentNullException( nameof(levelRegularBuilder) );
			_levelManager = levelManager ?? throw new ArgumentNullException( nameof(levelManager) );
			_gameManager = gameManager ?? throw new ArgumentNullException( nameof(gameManager) );
		}
		
		public void StartRegularLevel( LevelConfig levelConfig )
		{
			_levelRegularBuilder.SetConfig( levelConfig );
			_levelManager.StartLevel( levelConfig.scene.SceneName, _levelRegularBuilder,
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
			_levelManager.LeaveLevel();
		}
	}
}
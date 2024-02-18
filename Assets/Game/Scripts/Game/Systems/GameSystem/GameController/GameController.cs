using Game.Systems.LevelSystem;
using Game.Systems.UISystem;
using Zenject;

namespace Game.Systems.GameSystem
{
    public sealed class GameController : IInitializable
    {
	    private readonly LevelManager _levelManager;
	    private readonly UIRootGame _uiRootGame;
		private readonly SpawnSystem.SpawnSystem _spawnSystem;
		
	    public GameController(
		    LevelManager levelManager,
		    UIRootGame uiRootGame,
		    SpawnSystem.SpawnSystem spawnSystem
		    )
	    {
		    _levelManager = levelManager;
		    _uiRootGame = uiRootGame;
		    _spawnSystem = spawnSystem;
	    }

	    public void Initialize()
	    {
		    if ( _levelManager.CurrentLevel == null ) return;
		    
		    _uiRootGame.GameCanvas.SetLevel( _levelManager.CurrentLevel );
		    _spawnSystem.SpawnPlayer();
	    }
    }
}
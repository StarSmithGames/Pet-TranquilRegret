using Game.Managers.GameManager;
using Game.UI;

using StarSmithGames.Go;
using StarSmithGames.Go.SceneManager;

using Zenject;

namespace Game.Systems.SettingsSystem
{
    public class SettingsDialog : ViewBase
    {
		private UIGameCanvas subCanvas;

		private GameManager gameManager;

		private SceneManager sceneManager;

		[Inject]
		private void Construct(UIGameCanvas subCanvas, GameManager gameManager, SceneManager sceneManager)
		{
			this.subCanvas = subCanvas;
			this.gameManager = gameManager;
			this.sceneManager = sceneManager;
		}

		private void Awake()
		{
			subCanvas.ViewRegistrator.Registrate(this);
			Enable(false);
		}

		private void OnDestroy()
		{
			subCanvas.ViewRegistrator.UnRegistrate(this);
		}
		public void OnBackClick()
		{
			gameManager.ChangeState(gameManager.PreviousGameState);//PreGameplay, Gameplay
			Hide();
		}

		public void OnBackToMenuClick()
		{
			sceneManager.LoadSceneAsyncFromBuild(1, true);//Menu
		}
	}
}
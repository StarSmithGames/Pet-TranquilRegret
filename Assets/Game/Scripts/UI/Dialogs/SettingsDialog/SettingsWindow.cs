using Game.Managers.GameManager;
using Game.UI;

using StarSmithGames.Go;
using StarSmithGames.Go.SceneManager;

using Zenject;

namespace Game.Systems.SettingsSystem
{
    public class SettingsWindow : ViewBase
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

		private void Start()
		{
			Enable(false);

			subCanvas.ViewRegistrator.Registrate(this);
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
			sceneManager.LoadSceneAsyncFromBuild("Menu", false);
		}
	}
}
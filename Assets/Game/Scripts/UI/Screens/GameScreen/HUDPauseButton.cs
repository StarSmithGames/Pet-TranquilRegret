using Game.Managers.GameManager;
using Zenject;

namespace Game.UI
{
    public class HUDPauseButton : ViewHUD
	{
		private SettingsDialog settingsDialog;

		private UICanvas subCanvas;
		private GameManager gameManager;

		[Inject]
		private void Construct(
			UICanvas subCanvas,
			GameManager gameManager)
		{
			this.subCanvas = subCanvas;
			this.gameManager = gameManager;
		}

		private void Start()
		{
			settingsDialog = subCanvas.ViewRegistrator.GetAs<SettingsDialog>();
			settingsDialog.onShowingChanged += OnShowingChanged;
		}

		private void OnDestroy()
		{
			if (settingsDialog != null)
			{
				settingsDialog.onShowingChanged -= OnShowingChanged;
			}
		}

		public void OnClick()
		{
			settingsDialog.Show();
			gameManager.ChangeState(GameState.Pause);
		}

		private void OnShowingChanged(SettingsDialog dialog)
		{
			Enable(!dialog.IsShowing);
		}
	}
}
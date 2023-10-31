using Zenject;

namespace Game.UI
{
    public class HUDPauseButton : ViewHUD
	{
		[Inject]
		private UICanvas subCanvas;

		public void OnClick()
		{
			subCanvas.ViewRegistrator.Show<SettingsDialog>();
		}
	}
}
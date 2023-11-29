using Game.Services;

using Zenject;

namespace Game.UI
{
    public class HUDPauseButton : ViewHUD
	{
		[Inject]
		private ViewService viewService;

		public void OnClick()
		{
			viewService.CreateDialogIfNotExist<SettingsDialog>().Show();
		}
	}
}
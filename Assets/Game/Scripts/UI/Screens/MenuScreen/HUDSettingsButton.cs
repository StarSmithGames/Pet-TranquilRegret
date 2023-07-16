using UnityEngine;

using Zenject;

namespace Game.UI
{
	public class HUDSettingsButton : ViewHUD
	{
		private SettingsDialog settingsDialog;

		private UICanvas subCanvas;

		[Inject]
		private void Construct(UICanvas subCanvas)
		{
			this.subCanvas = subCanvas;
		}

		private void Start()
		{
			settingsDialog = subCanvas.ViewRegistrator.GetAs<SettingsDialog>();
			settingsDialog.onShowingChanged += OnShowingChanged;
		}

		private void OnDestroy()
		{
			if(settingsDialog != null)
			{
				settingsDialog.onShowingChanged -= OnShowingChanged;
			}
		}

		public void OnClick()
		{
			settingsDialog.Show();
		}

		private void OnShowingChanged(SettingsDialog dialog)
		{
			Enable(!dialog.IsShowing);
		}
	}
}
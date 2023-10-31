using UnityEngine;

using Zenject;

namespace Game.UI
{
	public class HUDSettingsButton : ViewHUD
	{
		private UICanvas subCanvas;

		[Inject]
		private void Construct(UICanvas subCanvas)
		{
			this.subCanvas = subCanvas;
		}

		public void OnClick()
		{
			subCanvas.ViewRegistrator.Show<SettingsDialog>();
		}
	}
}
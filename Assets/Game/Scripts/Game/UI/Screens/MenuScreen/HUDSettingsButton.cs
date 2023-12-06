using Game.Services;

using UnityEngine;

using Zenject;

namespace Game.UI
{
	public class HUDSettingsButton : ViewHUD
	{
		[Inject] private ViewService viewService;

		public void OnClick()
		{
			viewService.TryShowDialog<SettingsDialog>();
		}
	}
}
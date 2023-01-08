using Game.Managers.SceneManager;
using Game.UI;

using Zenject;

namespace Game.Systems.SettingsSystem
{
	public class UIBackToMenuButton : UIButton
	{
		private SceneManager sceneManager;

		[Inject]
		private void Construct(SceneManager sceneManager)
		{
			this.sceneManager = sceneManager;
		}

		protected override void OnClick()
		{
			base.OnClick();

			sceneManager.SwitchScene("Menu", false);
		}
	}
}
using Game.Managers.SceneManager;
using Game.UI;

using Zenject;

namespace Game.HUD.Menu
{
	public class UIStartButton : UIButton
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

			sceneManager.SwitchScene("Level_0", false);
		}
	}
}
using Game.UI;

using StarSmithGames.Go;

using Zenject;

namespace Game.Systems.SettingsSystem
{
    public class SettingsWindow : ViewBase
    {
		private UISubCanvas subCanvas;

		[Inject]
		private void Construct(UISubCanvas subCanvas)
		{
			this.subCanvas = subCanvas;
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
	}
}
using Game.UI;
using Zenject;

namespace Game.Systems.SettingsSystem
{
    public class SettingsWindow : WindowBase
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

			subCanvas.WindowsRegistrator.Registrate(this);
		}

		private void OnDestroy()
		{
			subCanvas.WindowsRegistrator.UnRegistrate(this);
		}
	}
}
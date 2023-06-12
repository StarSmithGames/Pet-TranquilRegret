using Game.Managers.LevelManager;
using Game.UI;

using Zenject;

namespace Game.Installers
{
	public class MenuInstaller : MonoInstaller
    {
		public UIMenuCanvas menuCanvas;

		public UIGoalItem goalPrefab;

		public override void InstallBindings()
		{
			Container
				.BindFactory<UIGoalItem, UIGoalItem.Factory>()
				.FromComponentInNewPrefab(goalPrefab)
				.AsSingle();

			Container.BindInstance(menuCanvas);
		}
	}
}
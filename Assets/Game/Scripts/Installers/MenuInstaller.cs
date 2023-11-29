using Game.Managers.GameManager;
using Game.Services;
using Game.Systems.CameraSystem;
using Game.Systems.InfinityRoadSystem;
using Game.UI;

using Zenject;

namespace Game.Installers
{
	public class MenuInstaller : MonoInstaller
    {
		public UIRootMenu menuRoot;

		public VerticalCamera verticalCamera;
		public RoadMap roadMap;

		public UIGoalItem goalPrefab;

		public override void InstallBindings()
		{
			Container
				.BindFactory<UIGoalItem, UIGoalItem.Factory>()
				.FromComponentInNewPrefab(goalPrefab)
				.AsSingle();

			Container.Bind<UIRootMenu>().FromInstance(menuRoot);

			Container.BindInstance(verticalCamera);
			Container.BindInstance(roadMap);

#if UNITY_EDITOR
			var gameManager = Container.Resolve<GameManager>();
			if (gameManager.CurrentGameState == GameState.Empty)
			{
				gameManager.ChangeState(GameState.Menu);
			}
#endif
		}
	}
}
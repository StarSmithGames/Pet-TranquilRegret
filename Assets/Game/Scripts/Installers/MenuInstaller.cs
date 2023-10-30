using Game.HUD.Menu;
using Game.Managers.GameManager;
using Game.Systems.CameraSystem;
using Game.Systems.InfinityRoadSystem;
using Game.UI;

using UnityEngine;

using Zenject;

namespace Game.Installers
{
	public class MenuInstaller : MonoInstaller
    {
		public UIMenuCanvas menuCanvas;

		public VerticalCamera verticalCamera;
		public RoadMap roadMap;

		public UIGoalItem goalPrefab;

		public override void InstallBindings()
		{
			Container
				.BindFactory<UIGoalItem, UIGoalItem.Factory>()
				.FromComponentInNewPrefab(goalPrefab)
				.AsSingle();

			Container.Bind<UICanvas>().FromInstance(menuCanvas);

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
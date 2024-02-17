using Game.Managers.GameManager;
using Game.Services;
using Game.Systems.CameraSystem;
using Game.Systems.InfinityRoadSystem;
using Game.UI;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
	public sealed class MenuInstaller : MonoInstaller< MenuInstaller >
    {
		public RoadMap roadMap;

		public UIGoalItem goalPrefab;
		
		[ Header("UI") ]
		public UIRootMenu UIRootMenu;

		public override void InstallBindings()
		{
			Container.Bind< UIRootMenu >().FromComponentsInNewPrefab( UIRootMenu ).AsSingle().NonLazy();
			
			Container
				.BindFactory<UIGoalItem, UIGoalItem.Factory>()
				.FromComponentInNewPrefab(goalPrefab)
				.AsSingle();

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
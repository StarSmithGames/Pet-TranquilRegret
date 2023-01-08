using Game.HUD;
using Game.Managers.CharacterManager;
using Game.Managers.LevelManager;
using Game.Systems.CameraSystem;
using Game.Systems.NavigationSystem;
using Game.UI;

using UnityEngine;

using Zenject;

namespace Game.Installers
{
	public class GameInstaller : MonoInstaller<GameInstaller>
	{
		public CameraSystem cameraSystem;
		public UISubCanvas subCanvas;
		public Joystick joystick;
		[Header("UI")]
		public UIGoal goalPrefab;

		public override void InstallBindings()
		{
			Container.BindInstance(cameraSystem);
			Container.BindInstance(subCanvas);
			Container.BindInstance(joystick);

			Container.BindFactory<UIGoal, UIGoal.Factory>()
				.FromComponentInNewPrefab(goalPrefab)
				.AsSingle();

			LevelManagerInstaller.Install(Container);
			CharacterManagerInstaller.Install(Container);
		}
	}
}
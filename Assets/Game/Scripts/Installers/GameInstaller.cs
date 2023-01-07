using Game.Managers.CharacterManager;
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

		public override void InstallBindings()
		{
			Container.BindInstance(cameraSystem);
			Container.BindInstance(subCanvas);
			Container.BindInstance(joystick);

			CharacterManagerInstaller.Install(Container);
		}
	}
}
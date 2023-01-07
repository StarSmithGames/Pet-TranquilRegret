using Game.Systems.NavigationSystem;
using Game.UI;
using UnityEngine;

using Zenject;

namespace Game.Installers
{
	public class GameInstaller : MonoInstaller<GameInstaller>
	{
		public UISubCanvas subCanvas;
		public Joystick joystick;

		public override void InstallBindings()
		{
			Container.BindInstance(subCanvas);
			Container.BindInstance(joystick);
		}
	}
}
using Game.Managers.AsyncManager;
using Game.Systems.ApplicationHandler;

using Zenject;

namespace Game.Installers
{
	public class ProjectInstaller : MonoInstaller<ProjectInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInstance(Container.InstantiateComponentOnNewGameObject<AsyncManager>());

			SignalBusInstaller.Install(Container);
			ApplicationHandlerInstaller.Install(Container);
		}
	}
}
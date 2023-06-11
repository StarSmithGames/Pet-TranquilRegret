using Game.Managers.GameManager;

using StarSmithGames.Go.ApplicationHandler;
using StarSmithGames.Go.AsyncManager;

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
			GameManagerInstaller.Install(Container);
		}
	}
}
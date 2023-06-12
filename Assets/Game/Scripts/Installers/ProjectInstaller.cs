using Game.Managers.GameManager;
using Game.Systems.GameSystem;

using StarSmithGames.Go.ApplicationHandler;
using StarSmithGames.Go.AsyncManager;
using StarSmithGames.Go.SceneManager;

using Zenject;

namespace Game.Installers
{
	public class ProjectInstaller : MonoInstaller<ProjectInstaller>
	{
		public GameplayConfig gameplayConfig;

		public override void InstallBindings()
		{
			Container.BindInstance(Container.InstantiateComponentOnNewGameObject<AsyncManager>());

			SignalBusInstaller.Install(Container);

			Container.BindInterfacesAndSelfTo<SceneManager>().AsSingle().NonLazy();
			ApplicationHandlerInstaller.Install(Container);
			GameManagerInstaller.Install(Container);

			Container.BindInstance(gameplayConfig).WhenInjectedInto<GameData>();
			Container.BindInterfacesAndSelfTo<GameData>().AsSingle().NonLazy();
		}
	}
}
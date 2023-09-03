using Game.Managers.CharacterManager;
using Game.Managers.GameManager;
using Game.Services;
using Game.Systems.GameSystem;
using Game.Systems.SpawnSystem;

using StarSmithGames.Go.ApplicationHandler;
using StarSmithGames.Go.SceneManager;
using StarSmithGames.IoC.AsyncManager;

using Zenject;

namespace Game.Installers
{
	public class ProjectInstaller : MonoInstaller<ProjectInstaller>
	{
		public GameplayConfig gameplayConfig;

		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<SceneManager>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<ViewService>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<VSFXService>().AsSingle().NonLazy();
			
			Container.BindInstance(gameplayConfig).WhenInjectedInto<GameData>();
			Container.BindInterfacesAndSelfTo<GameData>().AsSingle().NonLazy();

			SignalBusInstaller.Install(Container);
			AsyncManagerInstaller.Install(Container);
			ApplicationHandlerInstaller.Install(Container);
			GameManagerInstaller.Install(Container);
			CharacterManagerInstaller.Install(Container);
			SpawnSystemInstaller.Install(Container);

			Container.BindInstance(Container.InstantiateComponentOnNewGameObject<GameController>());
			Container.BindInterfacesAndSelfTo<GameLoader>().AsSingle().NonLazy();
		}
	}
}
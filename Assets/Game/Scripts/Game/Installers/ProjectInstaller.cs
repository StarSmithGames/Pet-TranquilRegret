using Game.Managers.CharacterManager;
using Game.Managers.GameManager;
using Game.Services;
using Game.Systems.GameSystem;
using Game.Systems.LevelSystem;
using Game.Systems.SpawnSystem;
using Game.Systems.StorageSystem;

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
#if !DISABLE_SRDEBUGGER
			Container.DeclareSignal<SignalOnLevelChangedCheat>();
#endif

			Container.BindInterfacesAndSelfTo<SceneManager>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<VSFXService>().AsSingle().NonLazy();

			Container.BindInstance(gameplayConfig).AsSingle();
			Container.BindInterfacesAndSelfTo<GameData>().AsSingle().NonLazy();

			SignalBusInstaller.Install(Container);
			AsyncManagerInstaller.Install(Container);
			ApplicationHandlerInstaller.Install(Container);
			GameManagerInstaller.Install(Container);
			SpawnSystemInstaller.Install(Container);

			Container.BindInstance(Container.InstantiateComponentOnNewGameObject<GameController>());
			Container.BindInterfacesAndSelfTo<GameLoader>().AsSingle().NonLazy();
		}
	}
}
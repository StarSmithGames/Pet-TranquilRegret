using Company.Module.Services.DelayedCallService;
using Game.Managers.GameManager;
using Game.Managers.PauseManager;
using Game.Managers.RewardManager;
using Game.Services;
using Game.Services.GameService;
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

			Container.BindInterfacesAndSelfTo<SceneManager>().AsSingle();
			Container.BindInterfacesAndSelfTo<VSFXService>().AsSingle();

			Container.BindInstance(gameplayConfig).AsSingle();

			SignalBusInstaller.Install(Container);
			AsyncManagerInstaller.Install(Container);
			ApplicationHandlerInstaller.Install(Container);
			StorageSystemInstaller.Install(Container);
			GameManagerInstaller.Install(Container);
			PauseManagerInstaller.Install(Container);
			LevelSystemInstaller.Install(Container);
			SpawnSystemInstaller.Install(Container);
			RewardManagerInstaller.Install(Container);
			DelayedCallServiceInstaller.Install(Container);

			Container.BindInterfacesAndSelfTo< GameController >().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo< GameLoader >().AsSingle();
			Container.BindInterfacesAndSelfTo< GameService >().AsSingle();
		}
	}
}
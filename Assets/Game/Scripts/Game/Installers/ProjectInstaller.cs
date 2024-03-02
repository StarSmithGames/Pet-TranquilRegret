using Company.Module.Services.DelayedCallService;
using Game.Managers.ApplicationManager;
using Game.Managers.AssetManager;
using Game.Managers.DIManager;
using Game.Managers.GameManager;
using Game.Managers.RewardManager;
using Game.Services;
using Game.Services.TickableService;
using Game.Systems.GameSystem;
using Game.Systems.LevelSystem;
using Game.Systems.SceneSystem;
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
			SignalBusInstaller.Install(Container);
			AsyncManagerInstaller.Install(Container);
			TickableServiceInstaller.Install( Container );
			ApplicationHandlerInstaller.Install(Container);
			ApplicationManagerInstaller.Install( Container );
			DelayedCallServiceInstaller.Install(Container);
			DIManagerInstaller.Install( Container );
			ResourcesManagerInstaller.Install( Container );
			StorageSystemInstaller.Install(Container);
			GameManagerInstaller.Install(Container);
			LevelSystemInstaller.Install(Container);
			RewardManagerInstaller.Install(Container);
			SceneSystemInstaller.Install( Container );
			
			Container.BindInterfacesAndSelfTo<VSFXService>().AsSingle();
			Container.BindInterfacesAndSelfTo< GameLoaderService >().AsSingle();
			Container.BindInterfacesAndSelfTo< GameService >().AsSingle();
			
			Container.BindInterfacesAndSelfTo< GamePipeline >().AsSingle().NonLazy();
			
			Container.BindInstance(gameplayConfig).AsSingle();

			Container.Resolve< DIManager >().SetContainer( Container );
		}
	}
}
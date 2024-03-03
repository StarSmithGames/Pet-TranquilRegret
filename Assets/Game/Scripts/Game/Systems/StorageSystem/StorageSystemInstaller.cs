using Zenject;

namespace Game.Systems.StorageSystem
{
	public class StorageSystemInstaller : Installer<StorageSystemInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<GameFastData>().AsSingle();
			
			Container.BindInterfacesAndSelfTo< StorageInitializerService >().AsSingle().WhenInjectedInto< StorageSystem >();
			Container.BindInterfacesAndSelfTo<StorageSystem>().AsSingle().NonLazy();
		}
	}
}
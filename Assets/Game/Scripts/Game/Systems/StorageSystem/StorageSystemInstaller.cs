using Zenject;

namespace Game.Systems.StorageSystem
{
	public class StorageSystemInstaller : Installer<StorageSystemInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<GameFastData>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<GamePlayData>().AsSingle().NonLazy();

			Container.BindInterfacesAndSelfTo<StorageSystem>().AsSingle().NonLazy();
		}
	}
}
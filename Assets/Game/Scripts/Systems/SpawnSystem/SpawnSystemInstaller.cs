using Zenject;

namespace Game.Systems.SpawnSystem
{
	public class SpawnSystemInstaller : Installer<SpawnSystemInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<SpawnSystem>().AsSingle().NonLazy();
		}
	}
}
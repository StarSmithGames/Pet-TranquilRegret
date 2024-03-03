using Zenject;

namespace Game.Services
{
	public sealed class OutlinableSystemInstaller : Installer< OutlinableSystemInstaller >
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo< OutlinableManager >().AsSingle();
			Container.BindInterfacesAndSelfTo< OutlinableService >().AsSingle();
			Container.BindInterfacesAndSelfTo< OutlinableTracker >().AsSingle().NonLazy();
		}
	}
}
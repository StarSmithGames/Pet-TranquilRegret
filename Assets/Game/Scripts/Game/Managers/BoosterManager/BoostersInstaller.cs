using Zenject;

namespace Game.Systems.BoosterManager
{
	public sealed class BoostersInstaller : Installer< BoostersInstaller >
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo< SpeedUpBooster >().AsSingle().WhenInjectedInto< BoosterManager >();
			Container.BindInterfacesAndSelfTo< VisionBooster >().AsSingle().WhenInjectedInto< BoosterManager >();
			Container.BindInterfacesAndSelfTo< BoosterManager >().AsSingle();
		}
	}
}
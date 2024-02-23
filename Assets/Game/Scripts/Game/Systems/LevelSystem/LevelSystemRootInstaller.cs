using Zenject;

namespace Game.Systems.LevelSystem
{
	public sealed class LevelSystemRootInstaller : Installer<LevelSystemRootInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo< LevelRegularService >().AsSingle();
			Container.BindInterfacesAndSelfTo< LevelManager >().AsSingle();
		}
	}
}
using Zenject;

namespace Game.Systems.LevelSystem
{
	public sealed class LevelSystemInstaller : Installer<LevelSystemInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo< LevelRegularService >().AsSingle();
			Container.BindInterfacesAndSelfTo< RegularLevelBuilder >().AsSingle();
			Container.BindInterfacesAndSelfTo< LevelManager >().AsSingle();
		}
	}
}
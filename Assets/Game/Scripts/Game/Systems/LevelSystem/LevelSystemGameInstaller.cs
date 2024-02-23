using Zenject;

namespace Game.Systems.LevelSystem
{
	public sealed class LevelSystemGameInstaller : Installer< LevelSystemGameInstaller >
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo< LevelRegularBuilder >().AsSingle();
		}
	}
}
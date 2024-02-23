using Zenject;

namespace Game.Systems.SceneSystem
{
	public sealed class SceneSystemInstaller : Installer< SceneSystemInstaller >
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo< SceneSystem >().AsSingle().NonLazy();
		}
	}
}
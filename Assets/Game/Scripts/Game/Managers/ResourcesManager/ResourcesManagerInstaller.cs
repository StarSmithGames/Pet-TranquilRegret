using Zenject;

namespace Game.Managers.AssetManager
{
	public sealed class ResourcesManagerInstaller : Installer< ResourcesManagerInstaller >
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo< ResourcesManager >().AsSingle().NonLazy();
		}
	}
}
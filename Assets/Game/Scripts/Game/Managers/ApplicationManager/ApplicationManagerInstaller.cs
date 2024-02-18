using Zenject;

namespace Game.Managers.ApplicationManager
{
	public sealed class ApplicationManagerInstaller : Installer< ApplicationManagerInstaller >
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo< ApplicationManager >().AsSingle().NonLazy();
		}
	}
}
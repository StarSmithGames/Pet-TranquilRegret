using Zenject;

namespace Game.Managers.DIManager
{
	public sealed class DIManagerInstaller : Installer< DIManagerInstaller >
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo< DIManager >().AsSingle();
		}
	}
}
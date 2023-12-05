using Zenject;

namespace Company.Module.Services.DelayedCallService
{
	public class DelayedCallServiceInstaller : Installer<DelayedCallServiceInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesTo<DelayedCallService>().AsSingle();
		}
	}
}
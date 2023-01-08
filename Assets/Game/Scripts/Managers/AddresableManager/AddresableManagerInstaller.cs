using Zenject;

namespace Game.Managers.AddresableManager
{
	public class AddresableManagerInstaller : Installer<AddresableManagerInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<AddresableManager>().AsSingle().NonLazy();
		}
	}
}
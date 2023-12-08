using Zenject;

namespace Game.Managers.PauseManager
{
	public class PauseManagerInstaller : Installer<PauseManagerInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<PauseManager>().AsSingle();
		}
	}
}
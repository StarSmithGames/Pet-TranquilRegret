using Zenject;

namespace Game.Managers.LevelManager
{
	public class LevelManagerInstaller : Installer<LevelManagerInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindFactory<LevelSettings, Level, Level.Factory>().WhenInjectedInto<LevelManager>();
			Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle().NonLazy();
		}
	}
}
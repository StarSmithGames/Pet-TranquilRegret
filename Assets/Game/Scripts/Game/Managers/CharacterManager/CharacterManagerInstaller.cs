using Game.Entity.CharacterSystem;

using Zenject;

namespace Game.Managers.CharacterManager
{
	public class CharacterManagerInstaller : Installer<CharacterManagerInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<CharacterManager>().AsSingle().NonLazy();
		}
	}
}
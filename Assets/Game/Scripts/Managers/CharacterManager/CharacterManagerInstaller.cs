using Zenject;

namespace Game.Managers.CharacterManager
{
	public class CharacterManagerInstaller : Installer<CharacterManagerInstaller>
	{
		public override void InstallBindings()
		{
			Container.DeclareSignal<SignalPlayerChanged>();

			Container.BindInterfacesAndSelfTo<CharacterManager>().AsSingle().NonLazy();
		}
	}

	public struct SignalPlayerChanged
	{
		public Player player;
	}
}
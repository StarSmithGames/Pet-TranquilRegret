using Game.Character;
using Game.Entities;

using UnityEngine;

using Zenject;

namespace Game.Systems.NavigationSystem
{
	[CreateAssetMenu(fileName = "NavigationSystemInstaller", menuName = "Installers/NavigationSystemInstaller")]
    public class NavigationSystemInstaller : ScriptableObjectInstaller<NavigationSystemInstaller>
    {
		public override void InstallBindings()
		{
			Container.Bind<ControlPresenter>().AsSingle().NonLazy();

			//Container.BindFactory<Player, PlayerGroundImplementation, PlayerGroundImplementation.Factory>().NonLazy();

			//Container
			//	.BindFactory<ICharacterModel, GroundImplementation, GroudImplementationFactory>()
			//	.FromFactory<CustomGroudImplementationFactory>()
			//	.NonLazy();
		}
	}
}
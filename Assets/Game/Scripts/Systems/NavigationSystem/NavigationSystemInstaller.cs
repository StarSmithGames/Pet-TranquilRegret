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
			Container.BindFactory<Player, PlayerGroundImplementation, PlayerGroundImplementation.Factory>().NonLazy();

			Container
				.BindFactory<ICharacter, GroundImplementation, GroudImplementationFactory>()
				.FromFactory<CustomGroudImplementationFactory>()
				.NonLazy();
		}
	}
}
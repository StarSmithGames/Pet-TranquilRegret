using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Systems.SpawnSystem
{
	[CreateAssetMenu(fileName = "SpawnSystemInstaller", menuName = "Installers/SpawnSystemInstaller")]
	public class SpawnSystemInstaller : ScriptableObjectInstaller<SpawnSystemInstaller>
	{
		public Player playerPrefab;

		public override void InstallBindings()
		{
			Container
				.BindFactory<Player, Player.Factory>()
				.FromComponentInNewPrefab(playerPrefab)
				.WhenInjectedInto<SpawnSystem>();

			Container.BindInterfacesAndSelfTo<SpawnSystem>().AsSingle().NonLazy();
		}
	}
}
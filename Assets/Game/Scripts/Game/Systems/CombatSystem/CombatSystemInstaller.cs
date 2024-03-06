using UnityEngine;
using Zenject;

namespace Game.Systems.CombatSystem
{
	[ CreateAssetMenu( fileName = "CombatSystemInstaller", menuName = "Installers/CombatSystemInstaller" ) ]
	public sealed class CombatSystemInstaller : ScriptableObjectInstaller< CombatSystemInstaller >
	{
		public SlashAttackVFX SlashAttackPrefab;
		
		public override void InstallBindings()
		{
			Container.BindFactory< SlashAttackVFX, SlashAttackVFXFactory >()
				.FromMonoPoolableMemoryPool( (pool) =>
					pool.WithInitialSize( 1 )
					.FromComponentInNewPrefab( SlashAttackPrefab ))
				.WhenInjectedInto< SlashAttack >();
		}
	}
}
using System;

namespace Game.Systems.CombatSystem
{
	public sealed class SlashAttack : Attack
	{
		private readonly SlashAttackVFXFactory _factory;
		
		public SlashAttack( SlashAttackVFXFactory factory )
		{
			_factory = factory ?? throw new ArgumentNullException( nameof(factory) );
		}
		
		public override AttackVFX Execute() => _factory.Create();
	}
}
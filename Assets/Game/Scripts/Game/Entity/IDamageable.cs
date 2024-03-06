using Game.Systems.CombatSystem;

namespace Game.Entity
{
	public interface IDamageable
	{
		void TakeDamage( Damage damage );
	}
}
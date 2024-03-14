using UnityEngine.Events;

namespace Game.Systems.CombatSystem
{
	public interface IDieable : IDamageable
	{
		event UnityAction<IDieable> OnDied;

		void Die();
	}
}
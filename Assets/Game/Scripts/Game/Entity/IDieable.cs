using UnityEngine.Events;

namespace Game.Entity
{
	public interface IDieable : IDamageable
	{
		event UnityAction<IDieable> OnDied;

		void Die();
	}
}
using System;

namespace Game.Systems.CombatSystem
{
	public interface IDestructible : IDamageable
	{
		event Action< IDestructible > OnDestructed;
		
		void Destruct();
	}
}
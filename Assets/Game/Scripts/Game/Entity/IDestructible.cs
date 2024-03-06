using System;

namespace Game.Entity
{
	public interface IDestructible : IDamageable
	{
		event Action< IDestructible > OnDestructed;
		
		void Destruct();
	}
}
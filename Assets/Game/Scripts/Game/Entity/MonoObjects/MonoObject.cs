using Game.Systems.CombatSystem;
using System;

namespace Game.Environment.EntitySystem
{
	public class MonoObject : EnvironmentObject, IDestructible
	{
		public event Action< IDestructible > OnDestructed;
		
		public virtual void TakeDamage( Damage damage )
		{
			
		}

		public virtual void Destruct()
		{
			gameObject.SetActive( false );
			
			OnDestructed?.Invoke( this );

			Dispose();
		}
	}
}
using Game.Environment.DestructableSystem;
using Game.Systems.CombatSystem;

namespace Game.Environment.EntitySystem
{
	public sealed class MonoHydrantObject : DestructableMonoObject
	{
		public override void TakeDamage( Damage damage )
		{
			Destruct();
		}
	}
}
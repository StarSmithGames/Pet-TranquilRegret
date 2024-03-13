using Game.Environment.DestructableSystem;
using Game.Systems.CombatSystem;

namespace Game.Environment.EntitySystem
{
	public sealed class MonoGlassObject : FracturableMonoObject
	{
		public override void TakeDamage( Damage damage )
		{
			Destruct();
		}
	}
}
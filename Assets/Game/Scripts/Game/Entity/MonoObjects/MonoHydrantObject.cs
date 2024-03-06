using Game.Systems.CombatSystem;

namespace Game.Environment.EntitySystem
{
	public sealed class MonoHydrantObject : MonoObject
	{
		public override void TakeDamage( Damage damage )
		{
			Destruct();
		}

		public override void Destruct()
		{
			//puff
			base.Destruct();
		}
	}
}
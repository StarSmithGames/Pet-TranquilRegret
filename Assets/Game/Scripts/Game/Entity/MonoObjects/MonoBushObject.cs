using Game.Systems.CombatSystem;
using UnityEngine;

namespace Game.Environment.EntitySystem
{
	public sealed class MonoBushObject : MonoObject
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
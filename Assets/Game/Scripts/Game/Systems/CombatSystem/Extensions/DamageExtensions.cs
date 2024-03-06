namespace Game.Systems.CombatSystem.Extensions
{
	public static class DamageExtensions
	{
		public static bool IsPhysicalDamage( this Damage damage ) => damage.DamageType.IsPhysicalDamage();
		
		public static bool IsPhysicalDamage( this DamageType damageType )
		{
			return damageType == DamageType.Slashing ||
			       damageType == DamageType.Crushing ||
			       damageType == DamageType.Piercing ||
			       damageType == DamageType.Missile;
		}
		
		public static bool IsMagicalDamage( this Damage damage ) => damage.DamageType.IsMagicalDamage();
		
		public static bool IsMagicalDamage( this DamageType damageType )
		{
			return damageType == DamageType.Magic ||
			       damageType == DamageType.Fire ||
			       damageType == DamageType.Air ||
			       damageType == DamageType.Water ||
			       damageType == DamageType.Cold ||
			       damageType == DamageType.Electricity ||
			       damageType == DamageType.Poison;
		}
	}
}
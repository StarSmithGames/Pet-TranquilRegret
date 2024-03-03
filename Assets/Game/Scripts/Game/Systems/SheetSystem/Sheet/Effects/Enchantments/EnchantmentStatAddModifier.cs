using StarSmithGames.Core;

namespace Game.Systems.SheetSystem.Effects
{
	public sealed class EnchantmentStatAddModifier : EnchantmentType
	{
		private readonly Stat _stat;
		private readonly AddModifier _modifier;

		public EnchantmentStatAddModifier( Stat stat, float add )
		{
			_stat = stat;
			_modifier = new( add );
		}

		public override void Activate()
		{
			base.Activate();
			_stat.AddModifier( _modifier );
		}

		public override void Deactivate()
		{
			base.Deactivate();
			_stat.RemoveModifier( _modifier );
		}
	}
}
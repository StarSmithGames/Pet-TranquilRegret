namespace Game.Systems.SheetSystem.Effects
{
	[System.Serializable]
	public sealed class AddMoveSpeed : Enchantment
	{
		private readonly EnchantmentStatAddModifier _enchantment;
		
		public AddMoveSpeed( MoveSpeedStat stat, float add )
		{
			_enchantment = new( stat, add );
		}

		public override void Activate()
		{
			base.Activate();
			_enchantment.Activate();
		}

		public override void Deactivate()
		{
			_enchantment.Deactivate();
			base.Deactivate();
		}
	}
}
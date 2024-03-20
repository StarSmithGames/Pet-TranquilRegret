namespace Game.Entity.CharacterSystem
{
	public class CharacterModel
	{
		public CharacterConfig Config { get; }
		public CharacterSheet Sheet { get; }

		public CharacterModel(CharacterConfig config)
		{
			Config = config;
			Sheet = new CharacterSheet(config.sheet);
		}
	}

	//public class CharacterSheet
	//{
	//	public MoveSpeed MoveSpeed { get; private set; }
	//	public JumpImpulse JumpImpulse { get; private set; }
	//	

	//	public Effects Effects { get; private set; }

	//	public CharacterSheet(ICharacterModel character)
	//	{
	//		MoveSpeed = new MoveSpeed(7.5f);
	//		JumpImpulse = new JumpImpulse(5);
	//		ThrowImpulse = new ThrowImpulse(7f);

	//		//Effects = new Effects(character);
	//	}
	//}
}
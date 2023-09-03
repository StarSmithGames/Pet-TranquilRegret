namespace Game.Character
{
    public class CharacterSheet
    {
        public MoveSpeed MoveSpeed { get; private set; }
        public JumpImpulse JumpImpulse { get; private set; }

		public CharacterSheet()
        {
            MoveSpeed = new MoveSpeed(1f);
            JumpImpulse = new JumpImpulse(1f);
		}
	}
}
using UnityEngine;

namespace Game.Character
{
    public class CharacterSheet
    {
        public MoveSpeed MoveSpeed { get; private set; }
        public JumpImpulse JumpImpulse { get; private set; }

		public CharacterSheet(CharacterSheetSettings settings)
        {
            MoveSpeed = new MoveSpeed(settings.defaultMoveSpeed);
            JumpImpulse = new JumpImpulse(settings.defaultJumpImpulse);
        }
    }

    [System.Serializable]
    public class CharacterSheetSettings
    {
        [Min(0)]
        public float defaultMoveSpeed = 1;
        [Min(0)]
		public float defaultJumpImpulse = 1;
	}
}
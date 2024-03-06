using Game.Systems.SheetSystem;
using UnityEngine;

namespace Game.Entity.CharacterSystem
{
    public sealed class CharacterSheet
    {
        public MoveSpeedStat MoveSpeed { get; private set; }
        public JumpImpulseStat JumpImpulse { get; private set; }

		public CharacterSheet(CharacterSheetSettings settings)
        {
            MoveSpeed = new MoveSpeedStat(settings.defaultMoveSpeed);
            JumpImpulse = new JumpImpulseStat(settings.defaultJumpImpulse);
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
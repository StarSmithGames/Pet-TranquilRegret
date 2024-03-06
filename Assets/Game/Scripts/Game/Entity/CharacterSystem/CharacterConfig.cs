using UnityEngine;

namespace Game.Entity.CharacterSystem
{
	[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Game/CharacterConfig")]
	public class CharacterConfig : ScriptableObject
	{
		public CharacterSheetSettings sheet;

		[Space]
		public CharacterController.Settings controllSettings;
	}
}
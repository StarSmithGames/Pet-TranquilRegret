using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character
{
	[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Game/CharacterConfig")]
	public class CharacterConfig : ScriptableObject
	{
		public CharacterSheetSettings sheet;

		[Space]
		public CharacterController.Settings controllSettings;
	}
}
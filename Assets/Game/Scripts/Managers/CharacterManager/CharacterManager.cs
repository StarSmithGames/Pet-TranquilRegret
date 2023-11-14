using Game.Character;

using StarSmithGames.Core;

using UnityEngine.Assertions;

namespace Game.Managers.CharacterManager
{
	public class CharacterManager
	{
		public AbstractCharacter Player { get; private set; }

		private Registrator<AbstractCharacter> registrator = new();

		public void Registrate(AbstractCharacter character, bool isPlayer)
		{
			if (isPlayer)
			{
				Assert.IsNull(Player);
				Player = character;
			}

			registrator.Registrate(character);
		}

		public void UnRegistrate(AbstractCharacter character)
		{
			registrator.UnRegistrate(character);
		}
	}
}
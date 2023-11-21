using StarSmithGames.Core;

using UnityEngine.Assertions;

namespace Game.Managers.CharacterManager
{
	public class CharacterManager
	{
		public Character.Character Player { get; private set; }
		public Registrator<Character.Character> Registrator { get; } = new();

		public void RegistratePlayer(Character.Character character)
		{
			Assert.IsNull(Player);
			Player = character;

			Registrator.Registrate(character);
		}

		public void UnRegistrate(Character.Character character)
		{
			Registrator.UnRegistrate(character);
		}
	}
}
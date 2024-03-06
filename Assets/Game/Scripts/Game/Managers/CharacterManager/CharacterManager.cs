using StarSmithGames.Core;

using UnityEngine.Assertions;

namespace Game.Managers.CharacterManager
{
	public class CharacterManager
	{
		public Entity.CharacterSystem.Character Player { get; private set; }
		public Registrator<Entity.CharacterSystem.Character> Registrator { get; } = new();

		public void RegistratePlayer(Entity.CharacterSystem.Character character)
		{
			Assert.IsNull(Player);
			Player = character;

			Registrator.Registrate(character);
		}

		public void UnRegistrate(Entity.CharacterSystem.Character character)
		{
			Registrator.UnRegistrate(character);
		}
	}
}
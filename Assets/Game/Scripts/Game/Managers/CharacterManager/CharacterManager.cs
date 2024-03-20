using StarSmithGames.Core;
using System;
using UnityEngine.Assertions;

namespace Game.Managers.CharacterManager
{
	public class CharacterManager
	{
		public event Action OnPlayerRegistrated; 
		
		public Entity.CharacterSystem.Character Player { get; private set; }
		public Registrator<Entity.CharacterSystem.Character> Registrator { get; } = new();

		public void RegistratePlayer(Entity.CharacterSystem.Character character)
		{
			Assert.IsNull(Player);
			Player = character;

			Registrator.Registrate(character);
			
			OnPlayerRegistrated?.Invoke();
		}

		public void UnRegistrate(Entity.CharacterSystem.Character character)
		{
			Registrator.UnRegistrate(character);
		}
	}
}
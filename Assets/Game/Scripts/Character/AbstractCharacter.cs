using UnityEngine;

using Zenject;

namespace Game.Character
{
	public abstract class AbstractCharacter : MonoBehaviour
	{
		public CharacterFacade facade;
		public Transform root;
		public Transform model;
		public CharacterSheet sheet;

		protected CharacterConfig config;

		[Inject]
		private void Construct(CharacterConfig config) 
		{
			this.config = config;

			sheet = new CharacterSheet(config.sheet);
		}

		public class Factory : PlaceholderFactory<AbstractCharacter> { }
	}
}
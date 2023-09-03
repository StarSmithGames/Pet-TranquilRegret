using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Character
{
    public abstract class AbstractCharacter : MonoBehaviour
	{
		public CharacterFacade facade;
		public Transform root;
		public Transform model;
		public CharacterSheet sheet = new CharacterSheet();

		public class Factory : PlaceholderFactory<AbstractCharacter> { }
	}
}
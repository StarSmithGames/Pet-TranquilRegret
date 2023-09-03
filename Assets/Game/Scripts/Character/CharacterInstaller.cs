using Game.Entities;
using Game.Systems.SheetSystem;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Character
{
	public class CharacterInstaller : MonoInstaller<CharacterInstaller>
	{
		public Rigidbody rigidbody;
		public Animator animator;

		[Space]
		public AbstractCharacter character;
		public CharacterController characterController;

		public override void InstallBindings()
		{
			Container.BindInstance(rigidbody);
			Container.BindInstance(animator);

			Container.BindInstance(character);
			Container.BindInstance(characterController);
		}
	}
}
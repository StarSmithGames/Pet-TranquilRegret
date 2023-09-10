using UnityEngine;

using Zenject;

namespace Game.Character
{
	public class CharacterInstaller : MonoInstaller<CharacterInstaller>
	{
		public Rigidbody rigidbody;
		public Animator animator;

		[Space]
		public CharacterConfig config;
		public AbstractCharacter character;
		public CharacterController characterController;
		public CharacterVSFXController vsfxController;
		[Space]
		public CharacterCanvas characerCanvas;

		public override void InstallBindings()
		{
			Container.BindInstance(rigidbody);
			Container.BindInstance(animator);

			Container.BindInstance(config);
			Container.BindInstance(character);
			Container.BindInstance(characterController);
			Container.BindInstance(vsfxController);
			Container.BindInstance(characerCanvas);

			Container.BindInterfacesAndSelfTo<CharacterGroundImplementation>().WhenInjectedInto<CharacterFacade>();
		}
	}
}
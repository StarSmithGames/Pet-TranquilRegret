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
		public CharacterView view;
		public CharacterController characterController;
		public CharacterVSFXController vsfxController;
		[Space]
		public CharacterCanvas characerCanvas;

		public override void InstallBindings()
		{
			Container.BindInstance(rigidbody);
			Container.BindInstance(animator);

			Container.BindInstance(characterController);
			Container.BindInstance(vsfxController);
			Container.BindInstance(characerCanvas);

			Container.Bind<CharacterPresenter>().AsSingle().NonLazy();
			Container.BindInstance(config).WhenInjectedInto<CharacterPresenter>();
			Container.BindInstance(view).WhenInjectedInto<CharacterPresenter>();
			Container.BindInterfacesAndSelfTo<CharacterGroundImplementation>().WhenInjectedInto<CharacterPresenter>();
		}
	}
}
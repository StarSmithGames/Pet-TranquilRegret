using UnityEngine;

using Zenject;

namespace Game.Entity.CharacterSystem
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

			Container.BindInstance( new CharacterModel( config ) );
			Container.BindInstance(view);
			Container.BindInstance(characterController);
			Container.BindInstance(vsfxController);
			Container.BindInstance(characerCanvas);
			// Container.BindInterfacesAndSelfTo<CharacterGroundImplementation>().AsSingle();
			
			CharacterCombatInstaller.Install( Container );
			
			Container.Bind<CharacterPresenter>().AsSingle().NonLazy();
		}
	}
}
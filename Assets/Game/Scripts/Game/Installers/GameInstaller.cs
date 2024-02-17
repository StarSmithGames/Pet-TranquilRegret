using Game.HUD.Gameplay;
using Game.Managers.CharacterManager;
using Game.Systems.CameraSystem;
using Game.Systems.PhysicsSystem;
using Game.Systems.PickupableSystem;
using Game.Systems.SheetSystem.Effects;
using Game.Systems.UISystem;
using UnityEngine;

using Zenject;

namespace Game.Installers
{
	public sealed class GameInstaller : MonoInstaller<GameInstaller>
	{
		public PhysicsSettings physicsSettings;

		[ Header("UI") ]
		public UIRootGame UIRootGame;
		public UIGoal goalPrefab;
		public UIPickup pickupPrefab;

		public override void InstallBindings()
		{
			Container.Bind< UIRootGame >().FromComponentsInNewPrefab( UIRootGame ).AsSingle().NonLazy();
			
			Container.BindInstance(FindObjectOfType<CameraSystem>());

			Container.BindInstance(physicsSettings);

			//Container.BindFactory<UIPickup, UIPickup.Factory>()
			//	.FromMonoPoolableMemoryPool(
			//	(x) => x.WithInitialSize(1)
			//	.FromComponentInNewPrefab(pickupPrefab)
			//	.UnderTransform(subCanvas.VFX));

			CharacterManagerInstaller.Install(Container);
			EffectSystemInstaller.Install(Container);
		}
	}
}
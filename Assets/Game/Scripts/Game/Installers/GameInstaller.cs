using Game.HUD.Gameplay;
using Game.Managers.CharacterManager;
using Game.Managers.PauseManager;
using Game.Systems.CameraSystem;
using Game.Systems.PhysicsSystem;
using Game.Systems.PickupableSystem;
using Game.Systems.SheetSystem.Effects;
using Game.Systems.SpawnSystem;
using Game.UI;

using UnityEngine;

using Zenject;

namespace Game.Installers
{
	public class GameInstaller : MonoInstaller<GameInstaller>
	{
		public PhysicsSettings physicsSettings;

		[Header("UI")]
		public UIRootGame uiRoot;
		public UIGoal goalPrefab;
		public UIPickup pickupPrefab;

		public override void InstallBindings()
		{
			Container.BindInstance(FindObjectOfType<CameraSystem>());

			Container.BindInstance(physicsSettings);

			Container.BindInstance(uiRoot);

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
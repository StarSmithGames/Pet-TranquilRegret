using Game.HUD.Gameplay;
using Game.Managers.CharacterManager;
using Game.Systems.CameraSystem;
using Game.Systems.PickupableSystem;
using Game.Systems.SheetSystem.Effects;
using Game.UI;

using UnityEngine;

using Zenject;

namespace Game.Installers
{
	public class GameInstaller : MonoInstaller<GameInstaller>
	{
		public UIGameCanvas subCanvas;
		[Header("UI")]
		public UIGoal goalPrefab;
		public UIPickup pickupPrefab;

		public override void InstallBindings()
		{
			Container.BindInstance(FindObjectOfType<CameraSystem>());
			Container.Bind<UICanvas>().FromInstance(subCanvas);

			Container.BindFactory<UIGoal, UIGoal.Factory>()
				.FromComponentInNewPrefab(goalPrefab)
				.AsSingle();

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
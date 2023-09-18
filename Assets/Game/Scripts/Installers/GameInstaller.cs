using Game.HUD.Gameplay;
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

			//Container.BindFactory<UIPickup, UIPickup.Factory>()
			//	.FromMonoPoolableMemoryPool(
			//	(x) => x.WithInitialSize(1)
			//	.FromComponentInNewPrefab(pickupPrefab)
			//	.UnderTransform(subCanvas.VFX));
			
			EffectSystemInstaller.Install(Container);
		}
	}
}
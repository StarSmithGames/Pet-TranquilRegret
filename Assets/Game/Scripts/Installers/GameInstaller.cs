using Game.HUD.Gameplay;
using Game.Managers.CharacterManager;
using Game.Managers.LevelManager;
using Game.Systems.CameraSystem;
using Game.Systems.NavigationSystem;
using Game.Systems.PickupableSystem;
using Game.UI;
using Game.VFX.Markers;
using Game.VFX;

using UnityEngine;

using Zenject;

namespace Game.Installers
{
	public class GameInstaller : MonoInstaller<GameInstaller>
	{
		public UISubCanvas subCanvas;
		[Header("UI")]
		public UIGoal goalPrefab;
		public UIPickup pickupPrefab;

		public override void InstallBindings()
		{
			Container.BindInstance(FindObjectOfType<CameraSystem>());
			Container.BindInstance(subCanvas).NonLazy();

			Container.BindFactory<UIGoal, UIGoal.Factory>()
				.FromComponentInNewPrefab(goalPrefab)
				.AsSingle();

			Container.BindFactory<UIPickup, UIPickup.Factory>()
				.FromMonoPoolableMemoryPool(
				(x) => x.WithInitialSize(1)
				.FromComponentInNewPrefab(pickupPrefab)
				.UnderTransform(subCanvas.VFX));

			LevelManagerInstaller.Install(Container);
			CharacterManagerInstaller.Install(Container);
		}
	}
}
using Game.HUD.Gameplay;
using Game.Managers.CharacterManager;
using Game.Managers.GameManager;
using Game.Systems.CameraSystem;
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
			
			EffectSystemInstaller.Install(Container);

#if UNITY_EDITOR
			var gameManager = Container.Resolve<GameManager>();
			if (gameManager.CurrentGameState == GameState.Empty)
			{
				gameManager.ChangeState(GameState.PreGameplay);
			}
#endif
		}
	}
}
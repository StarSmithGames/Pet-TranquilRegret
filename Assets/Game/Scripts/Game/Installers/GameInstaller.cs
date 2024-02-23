using Game.Managers.CharacterManager;
using Game.Managers.DIManager;
using Game.Managers.PauseManager;
using Game.Systems.CameraSystem;
using Game.Systems.LevelSystem;
using Game.Systems.PhysicsSystem;
using Game.Systems.SheetSystem.Effects;
using Game.Systems.SpawnSystem;
using Game.Systems.UISystem;
using Game.UI;
using UnityEngine;

using Zenject;

namespace Game.Installers
{
	public sealed class GameInstaller : MonoInstaller<GameInstaller>
	{
		public PhysicsSettings physicsSettings;

		[ Header("UI") ]
		public UIRootGame UIRootGame;
		public UIGoal UIGoalPrefab;

		public override void InstallBindings()
		{
			Container.Bind< UIRootGame >().FromComponentsInNewPrefab( UIRootGame ).AsSingle().NonLazy();

			Container.BindInstance(physicsSettings);

			Container.BindFactory< UIGoal, UIGoal.Factory >().FromComponentInNewPrefab( UIGoalPrefab );

			Container.BindInstance( FindObjectOfType< CameraSystem >() );
			SpawnSystemInstaller.Install(Container);
			EffectSystemInstaller.Install(Container);
			PauseManagerInstaller.Install(Container);
			CharacterManagerInstaller.Install(Container);
			
			Container.Resolve< DIManager >().SetContainer( Container );
		}
	}
}
using Cysharp.Threading.Tasks;
using Game.Systems.FloatingSystem;
using Game.Systems.LevelSystem;
using Game.Systems.NavigationSystem;
using UnityEngine;
using Zenject;

namespace Game.Systems.InventorySystem
{
	public class PickableFloatingItem : FloatingComponent
	{
		public CharacterInteractionZone interactionZone;

		[Inject] private LevelManager levelManager;

		private void Awake()
		{
			Subsctibe();
		}

		private void Subsctibe()
		{
			interactionZone.onItemAdded += OnCharacterAdded;
		}

		private void Unsubscribe()
		{
			interactionZone.onItemAdded -= OnCharacterAdded;
		}

		private void OnCharacterAdded(Character.Character character)
		{
			Unsubscribe();

			var config = GetComponent< ItemView >().model.config;
			if ( config == null )
			{
				Debug.LogError( $"[Asset] Config equal Null {gameObject.name}" );
				return;
			}
			var goal = config as GoalItemConfig;
			levelManager.CurrentLevel.Presenter.Gameplay.GoalRegistrator.AccumulatePrimaryGoal(goal);

			DoAnimationAsync(character.transform);
		}
	}
}
using Game.Systems.FloatingSystem;
using Game.Systems.LevelSystem;
using Game.Systems.NavigationSystem;
using Game.Systems.StorageSystem;

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

			var goal = GetComponent<ItemView>().model.config as GoalItemConfig;
			levelManager.CurrentLevel.Model.GoalRegistrator.AccumulatePrimaryGoal(goal);

			DoAnimationAsync(character.transform);
		}
	}
}
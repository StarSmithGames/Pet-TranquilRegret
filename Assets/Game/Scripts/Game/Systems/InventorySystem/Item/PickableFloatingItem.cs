using Game.Systems.FloatingSystem;
using Game.Systems.NavigationSystem;
using Game.Systems.StorageSystem;

using Zenject;

namespace Game.Systems.InventorySystem
{
	public class PickableFloatingItem : FloatingComponent
	{
		public CharacterInteractionZone interactionZone;

		[Inject] private StorageSystem.StorageSystem gameData;

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
			gameData.IntermediateData.LevelPresenter.Model.GoalRegistrator.AccumulatePrimaryGoal(goal);

			DoAnimationAsync(character.transform);
		}
	}
}
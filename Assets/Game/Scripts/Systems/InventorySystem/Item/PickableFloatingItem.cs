using Game.Systems.FloatingSystem;
using Game.Systems.NavigationSystem;

namespace Game.Systems.InventorySystem
{
	public class PickableFloatingItem : FloatingComponent
	{
		public CharacterInteractionZone interactionZone;

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

			DoAnimationAsync(character.transform);
		}
	}
}
using Game.Systems.InteractionSystem;
using Game.Systems.NavigationSystem;
using Game.Systems.PickupableSystem;

using UnityEngine;

using Zenject;

namespace Game.Entities
{
	public interface ICharacter
	{
		Systems.NavigationSystem.CharacterController Controller { get; }

		Effects Effects { get; }
	}

	public class Player : MonoBehaviour, ICharacter
	{
		public Effects Effects { get; private set; }

		[field: SerializeField] public Transform Model { get; private set; }
		[field: Space]
		[field: SerializeField] public PlayerVFX PlayerVFX { get; private set; }
		[field: SerializeField] public PlayerAvatar PlayerAvatar { get; private set; }
		[field: Space]
		[field: SerializeField] public Systems.NavigationSystem.CharacterController Controller { get; private set; }
		[field: Space]
		[field: SerializeField] public PlayerCanvas PlayerCanvas { get; private set; }

		private GroundImplementation groundImplementation;

		[Inject]
		private void Construct(GroudImplementationFactory groudImplementationFactory)
		{
			groundImplementation = groudImplementationFactory.Create(this);

			Effects = new Effects(this);
		}

		public void Pickup(PickupableObject pickupable)
		{

		}

		public class Factory : PlaceholderFactory<Player> { }
	}
}
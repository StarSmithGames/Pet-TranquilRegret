using Game.Systems.InteractionSystem;
using Game.Systems.NavigationSystem;
using Game.Systems.PickupableSystem;
using Game.UI;

using UnityEngine;

using Zenject;

namespace Game.Entities
{
	public interface ICharacter
	{
		Systems.NavigationSystem.CharacterController Controller { get; }

		Effects Effects { get; }
	}

	public partial class Player : MonoBehaviour, ICharacter
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
		private UIGameCanvas subCanvas;

		[Inject]
		private void Construct(UISubCanvas subCanvas, GroudImplementationFactory groudImplementationFactory)
		{
			this.subCanvas = subCanvas as UIGameCanvas;
			groundImplementation = groudImplementationFactory.Create(this);

			Effects = new Effects(this);
		}

		private void Start()
		{
			subCanvas.Drop.onClicked += OnDropClicked;
		}

		public class Factory : PlaceholderFactory<Player> { }
	}

	public partial class Player
	{
		public bool IsHandsEmpty => ObjectInHands == null;
		public PickupableObject ObjectInHands { get; private set; }

		public void Pickup(PickupableObject pickupable)
		{
			ObjectInHands = pickupable;

			ObjectInHands.transform.SetParent(PlayerAvatar.BothHandsPoint);
			ObjectInHands.transform.localPosition = Vector3.zero;
			ObjectInHands.transform.forward = PlayerAvatar.BothHandsPoint.forward;

			subCanvas.Drop.Show();
		}

		public void DropHandsObject()
		{
			if (!IsHandsEmpty)
			{
				ObjectInHands.transform.SetParent(null);
				ObjectInHands.Enable(true);

				ObjectInHands = null;
			}
		}

		private void OnDropClicked()
		{
			subCanvas.Drop.Hide();
			DropHandsObject();
		}
	}
}
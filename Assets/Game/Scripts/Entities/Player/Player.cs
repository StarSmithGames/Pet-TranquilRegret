using Game.Systems.InteractionSystem;
using Game.Systems.NavigationSystem;
using Game.Systems.PickupableSystem;
using Game.Systems.SheetSystem;
using Game.Systems.SheetSystem.Effects;
using Game.UI;

using UnityEngine;
using UnityEngine.Events;

using Zenject;

namespace Game.Entities
{
	public interface ICharacter
	{
		Systems.NavigationSystem.CharacterController Controller { get; }

		PlayerSheet Sheet { get; }
	}

	public partial class Player : MonoBehaviour, ICharacter
	{
		public PlayerSheet Sheet { get; private set; }

		[field: SerializeField] public Transform Model { get; private set; }
		[field: Space]
		[field: SerializeField] public PlayerVFX PlayerVFX { get; private set; }
		[field: SerializeField] public PlayerAvatar PlayerAvatar { get; private set; }
		[field: Space]
		[field: SerializeField] public Systems.NavigationSystem.CharacterController Controller { get; private set; }
		[field: Space]
		[field: SerializeField] public PlayerCanvas PlayerCanvas { get; private set; }

		private UIGameCanvas subCanvas;
		private GroundImplementation groundImplementation;

		[Inject]
		private void Construct(UISubCanvas subCanvas,
			GroudImplementationFactory groudImplementationFactory)
		{
			this.subCanvas = subCanvas as UIGameCanvas;
			groundImplementation = groudImplementationFactory.Create(this);

			Sheet = new PlayerSheet(this);
		}

		private void Start()
		{
			subCanvas.Drop.onClicked += OnDropClicked;
		}

		public class Factory : PlaceholderFactory<Player> { }
	}

	public partial class Player
	{
		public event UnityAction<PickupableObject> onObjectInHandsChanged;

		public bool IsHandsEmpty => ObjectInHands == null;
		public PickupableObject ObjectInHands { get; private set; }

		public void Pickup(PickupableObject pickupable)
		{
			ObjectInHands = pickupable;

			ObjectInHands.transform.SetParent(PlayerAvatar.BothHandsPoint);
			ObjectInHands.transform.localPosition = Vector3.zero;
			ObjectInHands.transform.forward = PlayerAvatar.BothHandsPoint.forward;

			subCanvas.Drop.Show();

			onObjectInHandsChanged?.Invoke(pickupable);
		}

		public void DropHandsObject()
		{
			if (!IsHandsEmpty)
			{
				ObjectInHands.transform.SetParent(null);
				ObjectInHands.Enable(true);
				ObjectInHands.Rigidbody.AddForce(Vector3.Lerp(Model.forward, transform.up, 0.5f) * Sheet.ThrowImpulse.TotalValue, ForceMode.Impulse);
				ObjectInHands = null;

				onObjectInHandsChanged?.Invoke(null);
			}
		}

		private void OnDropClicked()
		{
			subCanvas.Drop.Hide();
			DropHandsObject();
		}
	}


	public class PlayerSheet
	{
		public MoveSpeed MoveSpeed { get; private set; }
		public JumpImpulse JumpImpulse { get; private set; }
		public ThrowImpulse ThrowImpulse { get; private set; }

		public Effects Effects { get; private set; }

		public PlayerSheet(ICharacter character)
		{
			MoveSpeed = new MoveSpeed(7.5f);
			JumpImpulse = new JumpImpulse(5);
			ThrowImpulse = new ThrowImpulse(7f);

			Effects = new Effects(character);
		}
	}
}
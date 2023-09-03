using Game.Systems.PickupableSystem;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Character
{
	public partial class Character : AbstractCharacter
	{
		//private UIGameCanvas subCanvas;

		//[Inject]
		//private void Construct(UICanvas subCanvas)
		//{
		//	this.subCanvas = subCanvas as UIGameCanvas;
		//}

		//private void Start()
		//{
		//	subCanvas.Drop.onClicked += OnDropClicked;
		//}
	}

	//public partial class Player
	//{
	//	public event UnityAction<PickupableObject> onObjectInHandsChanged;

	//	public bool IsHandsEmpty => ObjectInHands == null;
	//	public PickupableObject ObjectInHands { get; private set; }

	//	public void Pickup(PickupableObject pickupable)
	//	{
	//		if (!IsHandsEmpty)
	//		{
	//			//Sheet.ThrowImpulse.Enable(false);
	//			DropHandsObject();
	//		}

	//		if (!IsHandsEmpty)
	//		{
	//			return;
	//		}

	//		ObjectInHands = pickupable;
	//		ObjectInHands.Enable(false);
	//		//ObjectInHands.transform.SetParent(PlayerAvatar.BothHandsPoint);
	//		ObjectInHands.transform.localPosition = Vector3.zero;
	//		//ObjectInHands.transform.forward = PlayerAvatar.BothHandsPoint.forward;

	//		//subCanvas.Drop.Show();

	//		onObjectInHandsChanged?.Invoke(pickupable);
	//	}

	//	public void DropHandsObject()
	//	{
	//		if (!IsHandsEmpty)
	//		{
	//			ObjectInHands.transform.SetParent(null);
	//			ObjectInHands.Enable(true);
	//			//ObjectInHands.Rigidbody.AddForce(Vector3.Lerp(model.forward, transform.up, 0.5f) * Sheet.ThrowImpulse.TotalValue, ForceMode.Impulse);
	//			ObjectInHands = null;

	//			onObjectInHandsChanged?.Invoke(null);
	//		}
	//	}

	//	public void AutoDropClick()
	//	{
	//		OnDropClicked();
	//	}

	//	private void OnDropClicked()
	//	{
	//		//Sheet.ThrowImpulse.Enable(true);
	//		//subCanvas.Drop.Hide();
	//		DropHandsObject();
	//	}
	//}
}
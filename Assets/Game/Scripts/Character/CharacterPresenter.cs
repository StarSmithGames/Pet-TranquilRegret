using Cysharp.Threading.Tasks;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;

using Zenject;

namespace Game.Character
{
	public class CharacterPresenter
	{
		public CharacterView View { get; private set; }
		public CharacterModel Model { get; private set; }

		[Inject] private CharacterGroundImplementation groundImplementation;

		public CharacterPresenter(CharacterView view, CharacterConfig config)
		{
			View = view;
			Model = new(config);
		}

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


		//public async void StartLockpickAsync()
		//{
		//	await Lockpickable(this.GetCancellationTokenOnDestroy());
		//}

		private async UniTask Lockpickable(CancellationToken cancellation)
		{
			await UniTask.Delay(TimeSpan.FromSeconds(0.33f), cancellationToken: cancellation);

			Debug.LogError("HERER");

			//if (!settings.isLocked) return;

			//if (currentCharacter != null)
			//{
			//	t += Time.deltaTime;
			//	progress = t / settings.unlockTime;

			//	currentCharacter.facade.characterCanvas.lockpick.FillAmount = progress;

			//	if (progress >= 1f)
			//	{
			//		settings.isLocked = false;
			//		onLockChanged?.Invoke(this);

			//	}
			//}
			//else
			//{
			//	if (progress != 0)
			//	{
			//		t -= Time.deltaTime * settings.decreaseSpeed;
			//		progress = Mathf.Max(t, 0) / settings.unlockTime;
			//	}
			//}

		}
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
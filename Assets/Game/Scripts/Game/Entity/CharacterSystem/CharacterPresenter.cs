using Cysharp.Threading.Tasks;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

using Zenject;

namespace Game.Entity.CharacterSystem
{
	public class CharacterPresenter
	{
		public CharacterModel Model { get; }
		public CharacterView View { get; }
		public CharacterCombat Combat { get; }

		[Inject] public CharacterController Controller { get; private set; }

		[Inject] private CharacterCanvas characterCanvas;

		[Inject] private CharacterGroundImplementation _characterGround;
		
		public CharacterPresenter(
			CharacterModel model,
			CharacterView view,
			CharacterCombat combat
			)
		{
			Model = model;
			View = view;
			Combat = combat;
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

		private UniTask lockpickTask;
		private CancellationTokenSource lockpickCancellation;

		public async void DoLockpickAsync()
		{
			//try
			//{
				lockpickCancellation?.Dispose();
				lockpickCancellation = new();
				if (await Lockpickable(lockpickCancellation.Token))
				{
					Debug.LogError("TRUE");
				}
				else
				{
					Debug.LogError("False");
				}
			//}catch(OperationCanceledException _) { }
		}

		public void BreakLockpick()
		{
			lockpickCancellation?.Cancel();
			lockpickCancellation?.Dispose();
			lockpickCancellation = null;
		}

		private async UniTask<bool> Lockpickable(CancellationToken cancellation)
		{
			characterCanvas.lockpick.Show();

			float t = 0;

			while (t < 10f)
			{
				t += Time.deltaTime;

				await UniTask.NextFrame();

				if (cancellation.IsCancellationRequested)
				{
					characterCanvas.lockpick.Hide();

					return false;
				}
			}

			//await UniTask.Delay(TimeSpan.FromSeconds(0.33f));

			characterCanvas.lockpick.Hide();

			return true;

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
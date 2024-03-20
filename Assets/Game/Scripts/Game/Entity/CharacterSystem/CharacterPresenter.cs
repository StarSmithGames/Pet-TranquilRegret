using Cysharp.Threading.Tasks;
using Game.Environment.PickableSystem;
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
		
		public CharacterPickupObserver PickupObserver { get; }

		[Inject] public CharacterController Controller { get; private set; }

		[Inject] private CharacterCanvas characterCanvas;

		[Inject] private CharacterGroundImplementation _characterGround;
		
		private UniTask lockpickTask;
		private CancellationTokenSource lockpickCancellation;
		
		public CharacterPresenter(
			CharacterModel model,
			CharacterView view,
			CharacterCombat combat
			)
		{
			Model = model;
			View = view;
			Combat = combat;

			PickupObserver = new( View.Points );
			PickupObserver.OnDropped += PickableObjectDropped;
		}

		private void PickableObjectDropped( PickableObject pickableObject )
		{
			pickableObject.Rigidbody.AddForce(Vector3.Lerp( View.Points.Rotor.forward, View.Points.Root.up, 0.5f) * 5f, ForceMode.Impulse);
		}
		
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
}
using DG.Tweening;

using StarSmithGames.Go;

using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Systems.LockpickingSystem
{
	public class UILockpick : ViewPopupBase
	{
		public float FillAmount
		{
			get => bar.fillAmount;
			set => bar.fillAmount = value;
		}

		public Image icon;
		public Image bar;
		[Space]
		public Sprite lockSprite;
		public Sprite unlockSprite;
		public Settigns settigns;

		private void Start()
		{
			Enable(false);
		}

		public override void Show(Action callback = null)
		{
			icon.rectTransform.anchoredPosition = Vector3.zero;
			icon.sprite = lockSprite;

			base.Show(() =>
			{
				StartLockAnimation();
				callback?.Invoke();
			});

			void StartLockAnimation()
			{
				DOTween.Sequence()
					.AppendCallback(() => icon.DORewind())
					.Append(icon.rectTransform.DOPunchScale(settigns.lockPunchStrength, settigns.lockPunchDuration))
					.Append(icon.rectTransform.DOShakePosition(settigns.lockShakeDuration, settigns.lockShakeStrength))
					.AppendInterval(0.35f)
					.SetLoops(-1);
			}
		}

		public override void Hide(Action callback = null)
		{
			Kill();
			base.Hide(callback);
		}

		public void Unlock(Action callback = null)
		{
			Kill();

			Sequence sequence = DOTween.Sequence();

			Vector2 start = icon.rectTransform.anchoredPosition;
			Vector2 end = icon.rectTransform.anchoredPosition + new Vector2(0, 30f);
			sequence
				.Append(icon.rectTransform.DOAnchorPos(end, 0.2f))
				.Append(icon.rectTransform.DOPunchScale(settigns.lockPunchStrength, settigns.lockPunchDuration))
				.AppendCallback(() => { icon.sprite = unlockSprite; })
				.Append(icon.rectTransform.DOAnchorPos(start, 0.15f))
				.AppendInterval(0.25f)
				.OnComplete(() =>
				{
					Hide(callback);
				});
		}

		private void Kill()
		{
			icon.DOKill(true);
		}

		[System.Serializable]
		public class Settigns
		{
			public Vector3 lockPunchStrength = new Vector3(1.5f, 1.5f, 0);
			public float lockPunchDuration = 0.25f;

			public Vector3 lockShakeStrength = new Vector3(1.5f, 1.5f, 0);
			public float lockShakeDuration = 0.5f;
		}
	} 
}
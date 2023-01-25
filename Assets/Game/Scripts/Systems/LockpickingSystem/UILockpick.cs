using DG.Tweening;
using Game.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Systems.LockpickingSystem
{
	public class UILockpick : WindowPopupBase
	{
		public float FillAmount
		{
			get => Bar.fillAmount;
			set => Bar.fillAmount = value;
		}

		[field: SerializeField] public Image Icon { get; private set; }
		[field: SerializeField] public Image Bar { get; private set; }

		[Space]
		[SerializeField] private Sprite lockSprite;
		[SerializeField] private Sprite unlockSprite;
		[SerializeField] private Settigns settigns;

		private RectTransform icon;

		private void Start()
		{
			Enable(false);

			icon = (Icon.transform as RectTransform);
		}

		public override void Show(UnityAction callback = null)
		{
			icon.anchoredPosition = Vector3.zero;
			Icon.sprite = lockSprite;

			base.Show(() =>
			{
				StartLockAnimation();
				callback?.Invoke();
			});
		}

		public override void Hide(UnityAction callback = null)
		{
			Kill();
			base.Hide(callback);
		}

		public void Unlock(UnityAction callback = null)
		{
			Kill();

			Sequence sequence = DOTween.Sequence();

			Vector2 start = icon.anchoredPosition;
			Vector2 end = icon.anchoredPosition + new Vector2(0, 30f);
			sequence
				.Append(icon.DOAnchorPos(end, 0.2f))
				.Append(icon.DOPunchScale(settigns.lockPunchStrength, settigns.lockPunchDuration))
				.AppendCallback(() => { Icon.sprite = unlockSprite; })
				.Append(icon.DOAnchorPos(start, 0.15f))
				.AppendInterval(0.25f)
				.OnComplete(() =>
				{
					Hide(callback);
				});
		}

		private void StartLockAnimation()
		{
			Sequence sequence = DOTween.Sequence();

			sequence
				.AppendCallback(() => icon.DORewind())
				.Append(icon.DOPunchScale(settigns.lockPunchStrength, settigns.lockPunchDuration))
				.Append(icon.DOShakePosition(settigns.lockShakeDuration, settigns.lockShakeStrength))
				.AppendInterval(0.35f)
				.SetLoops(-1);
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
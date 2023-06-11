using DG.Tweening;
using Game.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.HUD.Menu
{
	public class UIMenuTabButton : UIButton
	{
		public event UnityAction<UIMenuTabButton> onClicked;

		public bool IsSelected { get; private set; }

		[field: SerializeField] public UIAlert Alert { get; private set; }
		[field: SerializeField] public Image Icon { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Text { get; private set; }
		[field: SerializeField] public CanvasGroup TextCanvasGroup { get; private set; }
		[Space]
		[SerializeField] private Vector2 maxSize;
		[SerializeField] private Vector2 minSize;
		[SerializeField] private Color disabled;
		[SerializeField] private Color diselected;
		[SerializeField] private Color selected;

		private RectTransform rectIcon;
		private RectTransform rectText;

		protected override void Start()
		{
			Alert?.gameObject.SetActive(false);
		
			base.Start();
		}

		public override void Enable(bool trigger)
		{
			base.Enable(trigger);

			IsSelected = false;

			Icon.color = trigger ? IsSelected ? selected : diselected : disabled;
			TextCanvasGroup.Enable(false);
		}

		public void Select(bool animation = true)
		{
			CheckRefs();

			TextCanvasGroup.Enable(false);
			Text.color = selected;
			rectText.anchoredPosition = new Vector3(0, -100);

			if (animation)
			{
				Sequence sequence = DOTween.Sequence();

				sequence
					.Append(rectText.DOAnchorPosY(-50, 0.25f))
					.Join(TextCanvasGroup.DOFade(1f, 0.25f))
					.Join(rectIcon.DOAnchorPosY(50, 0.25f))
					.Join(rectIcon.DOSizeDelta(maxSize, 0.25f))
					.Join(Icon.DOColor(selected, 0.2f));
			}
			else
			{
				rectText.anchoredPosition = new Vector2(0, -50);
				TextCanvasGroup.alpha = 1f;
				rectIcon.anchoredPosition = new Vector2(0, 50);
				rectIcon.sizeDelta = maxSize;
				Icon.color = selected;
			}

			IsSelected = true;
		}

		public void Diselect(bool animation = true)
		{
			CheckRefs();

			if (animation)
			{
				Sequence sequence = DOTween.Sequence();

				sequence
					.Append(rectText.DOAnchorPosY(-100, 0.2f))
					.Join(TextCanvasGroup.DOFade(0f, 0.2f))
					.Join(rectIcon.DOAnchorPosY(0, 0.2f))
					.Join(rectIcon.DOSizeDelta(minSize, 0.2f))
					.Join(Icon.DOColor(diselected, 0.2f));
			}
			else
			{
				rectText.anchoredPosition = new Vector2(0, -100);
				TextCanvasGroup.alpha = 0f;
				rectIcon.anchoredPosition = new Vector2(0, 0);
				rectIcon.sizeDelta = minSize;
				Icon.color = diselected;
			}

			IsSelected = false;
		}

		private void CheckRefs()
		{
			if (rectText == null)
			{
				rectText = (Text.transform as RectTransform);
			}

			if (rectIcon == null)
			{
				rectIcon = (Icon.transform as RectTransform);
			}
		}

		protected override void OnClick()
		{
			base.OnClick();

			onClicked?.Invoke(this);
		}
	}
}
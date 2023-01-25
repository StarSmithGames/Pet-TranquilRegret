using Game.UI;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.PickupableSystem
{
	public class UIPickup : WindowPopupBasePoolable
	{
		public float FillAmount
		{
			get => Bar.fillAmount;
			set => Bar.fillAmount = value;
		}

		[field: SerializeField] public Image Icon { get; private set; }
		[field: SerializeField] public Image Bar { get; private set; }
		[field: SerializeField] public Button Button { get; private set; }

		private RectTransform icon;

		private void Start()
		{
			Enable(false);

			icon = (Icon.transform as RectTransform);

			Button.onClick.AddListener(OnClicked);
		}

		private void OnClicked()
		{
			Debug.LogError("HERER");
		}

		public class Factory : PlaceholderFactory<UIPickup> { }
	}
}
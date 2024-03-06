using Game.UI;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Zenject;

namespace Game.Environment.PickupableSystem
{
	public class UIPickup //: WindowPopupBasePoolable
	{
		//public event UnityAction onClicked;

		//public float FillAmount
		//{
		//	get => Bar.fillAmount;
		//	set => Bar.fillAmount = value;
		//}

		//[field: SerializeField] public Image Icon { get; private set; }
		//[field: SerializeField] public Image Bar { get; private set; }
		//[field: SerializeField] public Button Button { get; private set; }

		//private RectTransform rect;
		//private RectTransform icon;
		//private PickupableObject pickupable;


		//private CameraSystem.CameraSystem cameraSystem;

		//[Inject]
		//private void Construct(CameraSystem.CameraSystem cameraSystem)
		//{
		//	this.cameraSystem = cameraSystem;
		//}

		//private void Start()
		//{
		//	rect = transform as RectTransform;
		//	icon = (Icon.transform as RectTransform);

		//	Button.onClick.AddListener(OnClicked);

		//	rect.anchorMin = Vector2.zero;
		//	rect.anchorMax = Vector2.zero;
		//}

		//private void Update()
		//{
		//	if(pickupable != null)
		//	{
		//		rect.anchoredPosition = cameraSystem.Camera.WorldToScreenPoint(pickupable.PositionOffset);
		//	}
		//}

		//public void Show(PickupableObject pickupable, UnityAction callback = null)
		//{
		//	this.pickupable = pickupable;

		//	base.Show(callback);
		//}

		//public override void Hide(UnityAction callback = null)
		//{
		//	this.pickupable = null;

		//	base.Hide(callback);
		//}

		//private void OnClicked()
		//{
		//	onClicked?.Invoke();
		//}

		//public class Factory : PlaceholderFactory<UIPickup> { }
	}
}
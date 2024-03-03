using Game.Managers.CharacterManager;
using Game.Systems.NavigationSystem;
using Game.Systems.UISystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
	public sealed class UIControlCanvas : UICanvas
	{
		public UISpeedUpBooster SpeedUpBooster;
		public UIVisionBooster VisionBooster;
		[ Header("Control") ]
		public Button DropButton;
		public Button JumpButton;
		public UIJoystick Joystick;

		private CharacterManager _characterManager;

		[ Inject ]
		private void Construct(
			CharacterManager characterManager
			)
		{
			_characterManager = characterManager;
		}

		private void Start()
		{
			Joystick.OnTapped += OnJumpButtonClick;
		}

		private void OnJumpButtonClick( PointerEventData eventData )
		{
			_characterManager.Player.Presenter.Controller.Jump();
		}
		
		public void OnDropButtonClick()
		{
			
		}
	}
}
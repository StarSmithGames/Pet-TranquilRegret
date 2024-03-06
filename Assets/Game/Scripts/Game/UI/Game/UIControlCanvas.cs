using Game.Systems.NavigationSystem;
using Game.Systems.UISystem;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public sealed class UIControlCanvas : UICanvas
	{
		public event Action OnDropButtonClicked; 
		public event Action OnAttackButtonClicked; 
		
		public UISpeedUpBooster SpeedUpBooster;
		public UIVisionBooster VisionBooster;
		[ Header("Control") ]
		public Button DropButton;
		public Button AttackButton;
		public Button JumpButton;
		public UIJoystick Joystick;

		public void OnDropButtonClick()
		{
			OnDropButtonClicked?.Invoke();
		}
		
		public void OnAttackButtonClick()
		{
			OnAttackButtonClicked?.Invoke();
		}
	}
}
using Game.Managers.CharacterManager;
using Game.UI;
using UnityEngine;

using Zenject;

namespace Game.Systems.NavigationSystem
{
	public class ControlPresenter
	{
		[Inject] private CharacterManager characterManager;

		private UIGameCanvas gameCanvas;

		public ControlPresenter()
		{
			//gameCanvas = subCanvas as UIGameCanvas;
			
			//gameCanvas.joystick.onDragChanged
			//gameCanvas.drop.onClicked += OnDropped;
			//gameCanvas.jump.onClicked += OnJumped;
		}

		private void OnDropped()
		{

		}

		private void OnJumped()
		{
			//characterManager.Player.installer.characterController.Jump();
		}
	}
}
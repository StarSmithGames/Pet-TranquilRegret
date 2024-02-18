using Game.Managers.CharacterManager;
using Game.Systems.UISystem;
using Game.UI;
using Zenject;

namespace Game.Systems.NavigationSystem
{
	public class ControlPresenter
	{
		[Inject] private CharacterManager characterManager;

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
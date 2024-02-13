using Game.Managers.CharacterManager;

using UnityEngine.Events;

using Zenject;

namespace Game.UI
{
    public class HUDJumpButton : ViewHUD
    {
		public event UnityAction onClicked;

		[Inject] private CharacterManager characterManager;

		void Start()
        {
            Enable(true);
        }

		public void OnClick()
		{
			characterManager.Player.Presenter.Controller.Jump();

			onClicked?.Invoke();
		}
	}
}
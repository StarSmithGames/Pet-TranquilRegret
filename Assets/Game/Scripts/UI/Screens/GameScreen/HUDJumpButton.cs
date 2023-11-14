using UnityEngine.Events;

namespace Game.UI
{
    public class HUDJumpButton : ViewHUD
    {
		public event UnityAction onClicked;

		void Start()
        {
            Enable(true);
        }

		public void OnClick()
		{
			onClicked?.Invoke();
		}
	}
}
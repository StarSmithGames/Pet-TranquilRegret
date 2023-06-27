using UnityEngine.Events;

namespace Game.UI
{
    public class UIDropButton : ViewHUD
    {
		public event UnityAction onClicked;

		private void Start()
		{
			Enable(false);
		}

		public void OnClick()
		{
			onClicked?.Invoke();
		}
	}
}
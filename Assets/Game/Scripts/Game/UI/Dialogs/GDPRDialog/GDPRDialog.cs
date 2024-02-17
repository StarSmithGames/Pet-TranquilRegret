using Game.Services;

using StarSmithGames.Go;

using System;

using Zenject;

namespace Game.UI
{
    public class GDPRDialog : UIViewDialog
    {
        public event Action onAgreeClicked;

		public void OnTermsClick()
        {

        }

        public void OnPrivacyClick()
        {

        }

        public void OnAgreeClick()
        {
            onAgreeClicked?.Invoke();
		}
    }
}
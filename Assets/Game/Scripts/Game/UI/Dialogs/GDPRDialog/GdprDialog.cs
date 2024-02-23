using System;

namespace Game.UI
{
    public class GdprDialog : UIViewQuartDialog
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
using Game.Services;

using StarSmithGames.Go;

using System;

using Zenject;

namespace Game.UI
{
    public class GDPRDialog : ViewPopupBase
    {
        public event Action onAgreeClicked;

        [Inject] private ViewService viewService;

		private void Awake()
		{
            viewService.DialogViewRegistrator.Registrate(this);
		}

		private void OnDestroy()
		{
            viewService?.DialogViewRegistrator.UnRegistrate(this);
		}

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
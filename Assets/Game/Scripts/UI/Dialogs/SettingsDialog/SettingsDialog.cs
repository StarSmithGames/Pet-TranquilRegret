using Game.Managers.GameManager;
using Game.Systems.GameSystem;

using StarSmithGames.Go;
using StarSmithGames.Go.LocalizationSystem;

using System;

using UnityEngine.UI;

using Zenject;

namespace Game.UI
{
	public class SettingsDialog : ViewBase
    {
		public event Action<SettingsDialog> onShowingChanged;

		public TMPro.TextMeshProUGUI title;
		public Button backButton;

		private UICanvas subCanvas;
		private GameManager gameManager;
		private LocalizationSystem localizationSystem;
		[Inject] private GameLoader gameLoader;

		[Inject]
		private void Construct(
			UICanvas subCanvas,
			GameManager gameManager,
			LocalizationSystem localizationSystem)
		{
			this.subCanvas = subCanvas;
			this.gameManager = gameManager;
			this.localizationSystem = localizationSystem;
		}

		private void Awake()
		{
			localizationSystem.onLocalizationChanged += OnLocalizationChanged;

			subCanvas.ViewRegistrator.Registrate(this);
			Enable(false);
		}

		private void OnDestroy()
		{
			subCanvas.ViewRegistrator.UnRegistrate(this);

			if(localizationSystem != null)
			{
				localizationSystem.onLocalizationChanged -= OnLocalizationChanged;
			}
		}

		public override void Show(Action callback = null)
		{
			backButton.gameObject.SetActive(gameManager.IsGame);
			backButton.interactable = true;

			OnLocalizationChanged();

			base.Show(callback);
			onShowingChanged?.Invoke(this);
		}

		public override void Hide(Action callback = null)
		{
			base.Hide(() =>
			{
				onShowingChanged?.Invoke(this);
				callback?.Invoke();
			});
		}

		public void OnBackClick()
		{
			if(gameManager.CurrentGameState != GameState.Menu)
			{
				gameManager.ChangeState(gameManager.PreviousGameState);//PreGameplay, Gameplay
			}
			Hide();
		}

		public void OnBackToMenuClick()
		{
			backButton.interactable = false;

			gameLoader.LoadMenu();
		}

		private void OnLocalizationChanged()
		{
			title.text = gameManager.IsGame ? localizationSystem.Translate("ui.settings_dialog.title_game") : localizationSystem.Translate("ui.settings_dialog.title_menu");
		}
	}
}
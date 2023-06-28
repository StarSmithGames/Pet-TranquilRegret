using Game.Managers.GameManager;
using Game.UI;
using UnityEngine;

using Zenject;

namespace Game.Systems.SettingsSystem
{
    public class SettingsButtonHUD : ViewHUD
	{
		private SignalBus signalBus;
		private UIGameCanvas subCanvas;
		private GameManager gameManager;

		[Inject]
		private void Construct(SignalBus signalBus, UIGameCanvas subCanvas, GameManager gameManager)
		{
			this.signalBus = signalBus;
			this.subCanvas = subCanvas;
			this.gameManager = gameManager;
		}

		private void Awake()
		{
			signalBus?.Subscribe<SignalGameStateChanged>(OnGameStateChanged);
		}

		public void OnClick()
		{
			gameManager.ChangeState(GameState.Pause);
			subCanvas.ViewRegistrator.Show<SettingsWindow>();
			Enable(false);
		}

		private void OnGameStateChanged(SignalGameStateChanged signal)
		{
			if (gameManager.IsGame)
			{
				if (!IsShowing)
				{
					Enable(true);
				}
			}
		}
	}
}
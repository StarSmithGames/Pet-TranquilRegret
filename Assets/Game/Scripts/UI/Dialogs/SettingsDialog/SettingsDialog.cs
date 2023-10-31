using Game.Managers.GameManager;
using Game.Systems.GameSystem;
using Game.Systems.StorageSystem;

using StarSmithGames.Go;
using StarSmithGames.Go.LocalizationSystem;

using System;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.UI
{
	public class SettingsDialog : ViewPopupBase
	{
		public TMPro.TextMeshProUGUI title;
		public Button backButton;
		[Space]
		public PreferencesRadioButton music;
		public PreferencesRadioButton sound;
		public PreferencesRadioButton vibration;

		[Inject] private UICanvas subCanvas;
		[Inject] private GameManager gameManager;
		[Inject] private GameLoader gameLoader;
		[Inject] private GameData gameData;

		private void Awake()
		{
			subCanvas.ViewRegistrator.Registrate(this);
			Enable(false);

			AssignData();
		}

		private void OnDestroy()
		{
			subCanvas.ViewRegistrator.UnRegistrate(this);
		}

		public override void Show(Action callback = null)
		{
			backButton.interactable = true;

			base.Show(callback);
		}

		public override void Hide(Action callback = null)
		{
			base.Hide(() =>
			{
				callback?.Invoke();
			});
		}

		private void AssignData()
		{
			var data = gameData.PreferencesParams.GetData();

			if (gameData.IsFirstTime)
			{
				var settings = gameData.IntermediateData.GameplayConfig.preferences;

				music.Enable(settings.music);
				sound.Enable(settings.sound);
				vibration.Enable(settings.vibration);

				data.music = settings.music;
				data.sound = settings.sound;
				data.vibration = settings.vibration;
				gameData.PreferencesParams.SetData(data);
			}
			else
			{
				music.Enable(data.music);
				sound.Enable(data.sound);
				vibration.Enable(data.vibration);
			}
		}

		public void OnMusicClick()
		{
			var data = gameData.PreferencesParams.GetData();
			data.music = !data.music;
			music.Enable(data.music);
			gameData.PreferencesParams.SetData(data);
		}

		public void OnSoundClick()
		{
			var data = gameData.PreferencesParams.GetData();
			data.sound = !data.sound;
			sound.Enable(data.sound);
			gameData.PreferencesParams.SetData(data);
		}

		public void OnVibrationClick()
		{
			var data = gameData.PreferencesParams.GetData();
			data.vibration = !data.vibration;
			vibration.Enable(data.vibration);
			gameData.PreferencesParams.SetData(data);
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
	}
}
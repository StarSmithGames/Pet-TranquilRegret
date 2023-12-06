using Game.Managers.GameManager;
using Game.Services;
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
	public sealed class SettingsDialog : ViewPopupBase
	{
		public Button backButton;
		[Space]
		public UIRadioButton music;
		public UIRadioButton sound;
		public UIRadioButton vibration;
		public Button exitButton;

		[Inject] private ViewService viewService;
		[Inject] private StorageSystem storageSystem;
		[Inject] private GameManager gameManager;
		[Inject] private GameLoader gameLoader;

		private void Awake()
		{
			Enable(false);

			AssignData();
		}

		private void OnDestroy()
		{
			viewService.ViewDialogRegistrator.UnRegistrate(this);
		}

		public override void Show(Action callback = null)
		{
			exitButton.gameObject.SetActive(!gameManager.IsMenu);

			base.Show(callback);
		}

		private void AssignData()
		{
			var data = storageSystem.GameFastData.PreferencesParams.GetData();

			if (storageSystem.GameFastData.IsFirstTime)
			{
				var settings = storageSystem.IntermediateData.GameplayConfig.preferences;

				data.music = settings.music;
				data.sound = settings.sound;
				data.vibration = settings.vibration;
				storageSystem.GameFastData.PreferencesParams.SetData(data);
			}

			music.Enable(data.music);
			sound.Enable(data.sound);
			vibration.Enable(data.vibration);
		}

		public void OnMusicClick()
		{
			var data = storageSystem.GameFastData.PreferencesParams.GetData();
			data.music = !data.music;
			music.Enable(data.music);
			storageSystem.GameFastData.PreferencesParams.SetData(data);
		}

		public void OnSoundClick()
		{
			var data = storageSystem.GameFastData.PreferencesParams.GetData();
			data.sound = !data.sound;
			sound.Enable(data.sound);
			storageSystem.GameFastData.PreferencesParams.SetData(data);
		}

		public void OnVibrationClick()
		{
			var data = storageSystem.GameFastData.PreferencesParams.GetData();
			data.vibration = !data.vibration;
			vibration.Enable(data.vibration);
			storageSystem.GameFastData.PreferencesParams.SetData(data);
		}

		public void OnBackClick()
		{
			Hide();
		}

		public void OnExitClick()
		{
			gameLoader.LoadMenu();
		}
	}
}
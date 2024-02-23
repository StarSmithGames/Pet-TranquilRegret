using Game.Managers.GameManager;
using Game.Managers.PauseManager;
using Game.Services;
using Game.Systems.GameSystem;
using Game.Systems.StorageSystem;
using System;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.UI
{
	public sealed class SettingsDialog : UIViewDialog
	{
		public Button backButton;
		[Space]
		public UIRadioButton music;
		public UIRadioButton sound;
		public UIRadioButton vibration;
		public Button exitButton;

		[Inject] private StorageSystem storageSystem;
		[ Inject ] private GameplayConfig _gameplayConfig;
		[Inject] private GameManager gameManager;
		[Inject] private GameLoaderService _gameLoaderService;

		private void Awake()
		{
			Enable(false);

			AssignData();
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
				var settings = _gameplayConfig.preferences;

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
			music.DoAnimation(data.music);
			storageSystem.GameFastData.PreferencesParams.SetData(data);
		}

		public void OnSoundClick()
		{
			var data = storageSystem.GameFastData.PreferencesParams.GetData();
			data.sound = !data.sound;
			sound.DoAnimation(data.sound);
			storageSystem.GameFastData.PreferencesParams.SetData(data);
		}

		public void OnVibrationClick()
		{
			var data = storageSystem.GameFastData.PreferencesParams.GetData();
			data.vibration = !data.vibration;
			vibration.DoAnimation(data.vibration);
			storageSystem.GameFastData.PreferencesParams.SetData(data);
		}

		public void OnBackClick()
		{
			HideAndDispose();
		}

		public void OnExitClick()
		{
			_gameLoaderService.LoadMenu();
		}
	}
}
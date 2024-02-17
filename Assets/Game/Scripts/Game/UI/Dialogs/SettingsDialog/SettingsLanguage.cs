using Game.Systems.GameSystem;

using StarSmithGames.Go.LocalizationSystem;

using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.SettingsSystem
{
	public class SettingsLanguage : MonoBehaviour
	{
		public Image flag;
		public TMPro.TextMeshProUGUI country;
		public Button left;
		public Button right;

		[Inject] private StorageSystem.StorageSystem storageSystem;
		[Inject] private LocalizationSystem localizationSystem;
		[ Inject ] private GameplayConfig _gameplayConfig;

		private LocalizationSettins settins;

		private void Awake()
		{
			settins = _gameplayConfig.localizationSettins;

			localizationSystem.onLocalizationChanged += OnLocalizationChanged;
			OnLocalizationChanged();
		}

		private void OnDestroy()
		{
			if(localizationSystem != null)
			{
				localizationSystem.onLocalizationChanged -= OnLocalizationChanged;
			}
		}

		public void OnLeftClick()
		{
			left.interactable = false;
			right.interactable = false;

			storageSystem.GameFastData.LanguageIndex = (storageSystem.GameFastData.LanguageIndex - 1 + settins.flags.Count) % settins.flags.Count;
			UpdateLocalization();
		}

		public void OnRightClick()
		{
			left.interactable = false;
			right.interactable = false;

			storageSystem.GameFastData.LanguageIndex = (storageSystem.GameFastData.LanguageIndex + 1) % settins.flags.Count;
			UpdateLocalization();
		}

		private void UpdateLocalization()
		{
			localizationSystem.ChangeLocale(storageSystem.GameFastData.LanguageIndex);
		}

		private void OnLocalizationChanged()
		{
			left.interactable = true;
			right.interactable = true;

			var data = settins.flags.Find((x) => x.locale == localizationSystem.CurrentLocale);
			data = data ?? settins.flags.First();

			flag.sprite = data.sprite;
			country.text = data.country;
		}
	}
}
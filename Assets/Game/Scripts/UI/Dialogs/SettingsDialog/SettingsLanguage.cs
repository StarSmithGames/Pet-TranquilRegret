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

		[Inject] private GameData gameData;
		[Inject] private LocalizationSystem localizationSystem;

		private int currentIndex = -1;

		private void Awake()
		{
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

		}

		public void OnRightClick()
		{

		}

		private void OnLocalizationChanged()
		{
			var flags = gameData.GameplayConfig.localizationSettins.flags;
			var data = flags.Find((x) => x.locale == localizationSystem.CurrentLocale);
			data = data ?? flags.First();

			flag.sprite = data.sprite;
			country.text = data.country;
		}
	}
}
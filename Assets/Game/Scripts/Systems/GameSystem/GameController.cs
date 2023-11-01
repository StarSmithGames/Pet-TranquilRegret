using Game.Systems.StorageSystem;

using StarSmithGames.Go.ApplicationHandler;
using StarSmithGames.Go.LocalizationSystem;

using UnityEngine;

using Zenject;

namespace Game.Systems.GameSystem
{
    public class GameController : MonoBehaviour
    {
        [Inject] private GameData gameData;
        [Inject] private LocalizationSystem localizationSystem;
		[Inject] private SignalBus signalBus;

		private void Awake()
		{
			signalBus?.Subscribe<SignalOnApplicationFocusChanged>(OnApplicationChanged);
		}

		private void OnDestroy()
		{
			signalBus?.Unsubscribe<SignalOnApplicationFocusChanged>(OnApplicationChanged);
		}

		private void Start()
		{
			if (gameData.IsFirstTime)
			{
				gameData.LanguageIndex = localizationSystem.AvailableLocales.IndexOf(localizationSystem.CurrentLocale);
			}
			else
			{
				localizationSystem.ChangeLocale(gameData.LanguageIndex);
			}
			//ToMenu
		}


		private void OnApplicationChanged()
		{
			gameData.IsFirstTime = false;
		}
	}
}
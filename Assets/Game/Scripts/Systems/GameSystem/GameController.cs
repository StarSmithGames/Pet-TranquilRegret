using Game.Systems.GameSystem;

using StarSmithGames.Go.LocalizationSystem;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Systems.GameSystem
{
    public class GameController : MonoBehaviour
    {
        [Inject] private GameData gameData;
        [Inject] private LocalizationSystem localizationSystem;

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
		}
	}
}
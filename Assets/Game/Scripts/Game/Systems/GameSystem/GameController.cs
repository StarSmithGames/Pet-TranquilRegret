using Game.Extensions;
using Game.Managers.GameManager;
using Game.Services;
using Game.Systems.LevelSystem;
using Game.Systems.StorageSystem;

using StarSmithGames.Core.Utils;
using StarSmithGames.Go.ApplicationHandler;
using StarSmithGames.Go.LocalizationSystem;

using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using Zenject;

namespace Game.Systems.GameSystem
{
    public sealed class GameController : MonoBehaviour
    {
		private bool isFirstTime;

		[Inject] private LevelManager levelManager;
		[Inject] private GameplayConfig gameplayConfig;
		[Inject] private GameManager gameManager;
		[Inject] private ViewService viewService;
        [Inject] private StorageSystem.StorageSystem storageSystem;
        [Inject] private LocalizationSystem localizationSystem;
		[Inject] private SignalBus signalBus;
		[Inject] private StarSmithGames.Go.SceneManager.SceneManager sceneManager;

		private void Awake()
		{
			isFirstTime = storageSystem.GameFastData.IsFirstTime;

			CheckInterruptGameProcess();

			Debug.LogError("HERER");
			if (sceneManager.IsLevel())
			{
				var config = GetLevelConfigFull(sceneManager.GetActiveScene());
				levelManager.StartRegularLevel(config);
			}

			signalBus?.Subscribe<SignalOnApplicationFocusChanged>(OnApplicationChanged);
		}

		private void OnDestroy()
		{
			signalBus?.Unsubscribe<SignalOnApplicationFocusChanged>(OnApplicationChanged);
		}

		private void Start()
		{
			if (storageSystem.GameFastData.IsFirstTime)
			{
				storageSystem.GameFastData.LanguageIndex = localizationSystem.AvailableLocales.IndexOf(localizationSystem.CurrentLocale);
			}
			else
			{
				localizationSystem.ChangeLocale(storageSystem.GameFastData.LanguageIndex);
			}
			//ToMenu
		}

		private LevelConfig GetLevelConfigFull(Scene scene)
		{
			return gameplayConfig.GetAllLevels().Find((x) => x.scene.SceneName == scene.name);
		}

		private void CheckInterruptGameProcess()
		{
			CloseData closeParams = storageSystem.GameFastData.CloseParams.GetData();

			//if (closeParams.IsPromptValid)
			//{
			//	_analyticsSystem.push_custom(closeParams.promptLevelFinishName, closeParams.promptLevelFinish);
			//}
		}


		private void OnApplicationChanged(SignalOnApplicationFocusChanged signal)
		{
			if (!signal.trigger)
			{
				storageSystem.GameFastData.IsFirstTime = false;

				var data = new CloseData
				{
					//appCloseTimestampUTC = TimestampUtils.GetTimestamp(_ntpDateTimeService.Now.ToUniversalTime()),
					appCloseTimestampLocal = TimestampUtils.GetTimestamp(DateTime.Now.ToLocalTime()),
					isInterruptGameProcess = false
				};

				if (gameManager.IsGame)
				{
					if (!viewService.IsSafeDialogShowig())
					{
						//var level = levelManager.CurrentLevel;
						//var isSafeLevel = !level.Model.UseLives;// || level.Model.UseLives && _livesManager.IsInfiniteLives();
						//data.isInterruptGameProcess = !isSafeLevel;
					}

					//var items = storageSystem.IntermediateData.CurrentLevel.GetPromtInterruptedGameProccess();
					//data.promptLevelFinishName = items.Item1;
					//data.promptLevelFinish = items.Item2;
				}

				storageSystem.GameFastData.CloseParams.SetData(data);

				storageSystem.Save(); //required
			}
			else
			{
				if (isFirstTime)
				{
					storageSystem.GameFastData.IsFirstTime = true;
				}
			}
		}
	}
}
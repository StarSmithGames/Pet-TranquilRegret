using Game.Services;
using Game.Systems.LevelSystem;
using Game.Systems.StorageSystem;
using Game.UI;

using System.Collections;

using UnityEngine;

using Zenject;

namespace Game.Systems.GameSystem
{
	public sealed class Preloader : MonoBehaviour
    {
		private GDPRDialog dialog;

		[Inject] private StorageSystem.StorageSystem gameData;
		[Inject] private GameplayConfig gameplayConfig;
		[Inject] private ViewService viewService;
		[Inject] private GameLoader gameLoader;
		[Inject] private LevelManager levelManager;

		private void Start()
		{
			Debug.LogError(gameData.GameFastData.IsFirstTime);

			gameData.GameFastData.SessionsCount++;

			if (!gameData.GameFastData.IsGDPRApplied)
			{
				dialog = viewService.ViewDialogRegistrator.GetAs<GDPRDialog>();
				dialog.onAgreeClicked += OnAgreeClicked;
				dialog.Show();
			}
			else
			{
				LoadGame();
			}
		}

		private void OnDestroy()
		{
			if(dialog != null)
			{
				dialog.onAgreeClicked -= OnAgreeClicked;
			}
		}

		private void LoadGame()
		{
			if (gameData.GameFastData.IsFirstTime)
			{
				levelManager.StartRegularLevel(gameplayConfig.firstTimeTutorialLevel);
			}
			else
			{
				gameLoader.LoadMenu();
			}
		}

		private void OnAgreeClicked()
		{
			gameData.GameFastData.IsGDPRApplied = true;

			if (dialog != null)
			{
				dialog.onAgreeClicked -= OnAgreeClicked;
				dialog.Hide(() =>
				{
					LoadGame();
				});
			}
			else
			{
				LoadGame();
			}
		}
	}
}
using Game.Services;
using Game.Systems.StorageSystem;
using Game.UI;

using System.Collections;

using UnityEngine;

using Zenject;

namespace Game.Systems.GameSystem
{
	public class Preloader : MonoBehaviour
    {
		[Inject] private StorageSystem.StorageSystem gameData;
		[Inject] private ViewService viewService;
		[Inject] private GameLoader gameLoader;

		private GDPRDialog window;

		private void Start()
		{
			gameData.GameFastData.SessionsCount++;

			if (!gameData.GameFastData.IsGDPRApplied)
			{
				window = viewService.ViewDialogRegistrator.GetAs<GDPRDialog>();
				window.onAgreeClicked += OnAgreeClicked;
				window.Show();
			}
			else
			{
				LoadGame();
			}
		}

		private void OnDestroy()
		{
			if(window != null)
			{
				window.onAgreeClicked -= OnAgreeClicked;
			}
		}

		private void LoadGame()
		{
			gameLoader.LoadMenu();
		}

		private void OnAgreeClicked()
		{
			gameData.GameFastData.IsGDPRApplied = true;

			if (window != null)
			{
				window.onAgreeClicked -= OnAgreeClicked;
				window.Hide(() =>
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
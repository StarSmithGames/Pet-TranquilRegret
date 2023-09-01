using Game.Services;
using Game.UI;

using System.Collections;

using UnityEngine;

using Zenject;

namespace Game.Systems.GameSystem
{
	public class Preloader : MonoBehaviour
    {
		[Inject] private GameData gameData;
		[Inject] private ViewService viewService;
		[Inject] private GameLoader gameLoader;

		private GDPRDialog window;

		private IEnumerator Start()
		{
			gameData.SessionsCount++;

			if (!gameData.IsGDPRApplied)
			{
				window = viewService.DialogViewRegistrator.GetAs<GDPRDialog>();
				window.onAgreeClicked += OnAgreeClicked;
				yield return new WaitForSeconds(1f);
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
			gameData.IsGDPRApplied = true;

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
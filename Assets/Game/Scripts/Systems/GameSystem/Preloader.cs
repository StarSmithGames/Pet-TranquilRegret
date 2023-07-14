using Game.Managers.TransitionManager;
using Game.Services;
using Game.UI;

using StarSmithGames.Go.SceneManager;

using System.Transactions;

using UnityEngine;

using Zenject;

namespace Game.Systems.GameSystem
{
    public class Preloader : MonoBehaviour
    {
		[Inject] private GameData gameData;
		[Inject] private ViewService viewService;
		//[Inject] private TransitionManager transitionManager;

		private GDPRDialog window;

		private void Start()
		{
			gameData.SessionsCount++;

			if (!gameData.IsGDPRApplied)
			{
				window = viewService.DialogViewRegistrator.GetAs<GDPRDialog>();
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
			//transitionManager.StartInfinityLoadingSceneAsync(1);//Main Menu;
		}

		private void OnAgreeClicked()
		{
			gameData.IsGDPRApplied = true;

			LoadGame();
		}
	}
}
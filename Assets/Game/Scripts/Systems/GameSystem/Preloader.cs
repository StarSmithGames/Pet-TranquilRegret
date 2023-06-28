using Game.Services;
using Game.UI;
using UnityEngine;

using Zenject;

namespace Game.Systems.GameSystem
{
    public class Preloader : MonoBehaviour
    {
		[Inject] private GameData gameData;
		[Inject] private ViewService viewService;

		private GDPRDialog window;

		private void Start()
		{
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

		}

		private void OnAgreeClicked()
		{
			gameData.IsGDPRApplied = true;

			LoadGame();
		}
	}
}
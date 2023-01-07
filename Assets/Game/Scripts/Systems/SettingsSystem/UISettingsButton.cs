using Game.Managers.GameManager;
using Game.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.SettingsSystem
{
    public class UISettingsButton : UIButton
    {
		[SerializeField] private Image image;
		[SerializeField] private Sprite open;
		[SerializeField] private Sprite close;

		private bool isShowing = false;

		private UISubCanvas subCanvas;
		private GameManager gameManager;

		[Inject]
		private void Construct(UISubCanvas subCanvas, GameManager gameManager)
		{
			this.subCanvas = subCanvas;
			this.gameManager = gameManager;
		}

		protected override void OnClick()
		{
			isShowing = !isShowing;

			if (isShowing)
			{
				gameManager.ChangeState(GameState.Pause);
				subCanvas.WindowsRegistrator.Show<SettingsWindow>();
			}
			else
			{
				gameManager.ChangeState(gameManager.PreviousGameState);//PreGameplay, Gameplay
				subCanvas.WindowsRegistrator.Hide<SettingsWindow>();
			}

			image.sprite = isShowing ? close : open;

			base.OnClick();
		}
	}
}
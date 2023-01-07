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

		[Inject]
		private void Construct(UISubCanvas subCanvas)
		{
			this.subCanvas = subCanvas;
		}

		protected override void OnClick()
		{
			isShowing = !isShowing;

			if (isShowing)
			{
				subCanvas.WindowsRegistrator.Show<SettingsWindow>();
			}
			else
			{
				subCanvas.WindowsRegistrator.Hide<SettingsWindow>();
			}

			image.sprite = isShowing ? close : open;

			base.OnClick();
		}
	}
}
using StarSmithGames.Go;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.UI
{

    public sealed class RateUsDialog : ViewPopupBase
	{
        public List<UIRadioButton> stars = new();
		public UISimpleButton simpleButton;

		[Inject] private UICanvas subCanvas;

		private int currentRate = 0;

		private void Awake()
		{
			subCanvas.ViewRegistrator.Registrate(this);
			//Enable(false);
			UpdateUI();
		}

		private void OnDestroy()
		{
			subCanvas.ViewRegistrator.UnRegistrate(this);
		}

		private void UpdateUI()
		{
			for (int i = 0; i < stars.Count; i++)
			{
				stars[i].Enable(i < currentRate);
			}

			simpleButton.Enable(currentRate != 0);
		}

		public void OnRated(int index)
        {
			currentRate = index + 1;

			UpdateUI();
		}

        public void OnRateConfirmed()
        {
			Hide();
		}

		public void OnBackClick()
		{
			Hide();
		}
	}
}
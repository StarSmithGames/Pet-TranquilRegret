using StarSmithGames.Go;

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.HUD.Gameplay
{
    public class UIDropButton : ViewPopupBase
    {
		public event UnityAction onClicked;

        [field: SerializeField] public Button Button { get; private set; }

		private bool isInitialized = false;

		private void Start()
		{
			Enable(false);
		}

		public override void Show(Action callback = null)
		{
			if (!isInitialized)
			{
				Button.onClick.AddListener(OnClick);
			}

			base.Show(callback);
		}

		private void OnDestroy()
		{
			Button?.onClick.RemoveAllListeners();
		}

		private void OnClick()
		{
			onClicked?.Invoke();
		}
	}
}
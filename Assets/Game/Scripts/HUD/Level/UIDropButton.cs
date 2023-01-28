using Game.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.HUD.Gameplay
{
    public class UIDropButton : WindowPopupBase
    {
		public event UnityAction onClicked;

        [field: SerializeField] public Button Button { get; private set; }

		private bool isInitialized = false;

		private void Start()
		{
			Enable(false);
		}

		public override void Show(UnityAction callback = null)
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
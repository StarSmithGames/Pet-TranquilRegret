using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.HUD.Menu
{
    public class UIMenuBottom : MonoBehaviour
    {
        [field: SerializeField] UIMenuTabButton Chest;
        [field: SerializeField] UIMapButton Map;
		[field: SerializeField] UIMenuTabButton Gallery;

		private UIMenuTabButton last;

		private void Start()
		{
			Chest.Diselect(false);
			Map.Select(false);
			Gallery.Enable(false);

			last = Map;

			Chest.onClicked += OnClicked;
			Map.onClicked += OnClicked;
			Gallery.onClicked += OnClicked;
		}

		private void OnClicked(UIMenuTabButton button)
		{
			if (!button.IsEnable) return;
			if (last == button) return;

			last?.Diselect();
			last = button;
			if (!last.IsSelected)
			{
				last.Select();
			}
		}
	}
}
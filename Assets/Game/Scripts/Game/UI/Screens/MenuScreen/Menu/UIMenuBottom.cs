using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.HUD.Menu
{
    public class UIMenuBottom : MonoBehaviour
    {
        public UIMapButton map;

		private UIMenuTabButton last;

		private void Start()
		{
			//Chest.Diselect(false);
			//Map.Select(false);
			//Gallery.Enable(false);
		}

		//private void OnClicked(UIMenuTabButton button)
		//{
		//	//if (!button.IsEnable) return;
		//	if (last == button) return;

		//	last?.Diselect();
		//	last = button;
		//	if (!last.IsSelected)
		//	{
		//		last.Select();
		//	}
		//}
	}
}
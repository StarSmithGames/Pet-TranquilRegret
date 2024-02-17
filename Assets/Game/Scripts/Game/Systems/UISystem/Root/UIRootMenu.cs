using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public sealed class UIRootMenu : UIRoot
	{
		public UINavigationCanvas NavigationCanvas;
        public UIDynamicCanvas DynamicCanvas;
        public UIFrontCanvas FrontCanvas;

        public List<Canvas> canvases = new();

        public void ApplyCamera( Camera camera )
        {
            for (int i = 0; i < canvases.Count; i++)
            {
                canvases[i].worldCamera = camera;
            }
        }

        public override Transform GetDialogsRoot() => DynamicCanvas.dialogsRoot;
	}
}
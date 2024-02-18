using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Systems.UISystem
{
    public sealed class UIRootMenu : UIRoot
	{
		public ViewAggregator DialogAggregator { get; private set; }
		
		public UINavigationCanvas NavigationCanvas;
        public UIDynamicCanvas DynamicCanvas;
        public UIFrontCanvas FrontCanvas;

        public List<Canvas> canvases = new();
        
        [ Inject ]
        private void Construct(
	        DiContainer container,
	        UISettings uiSettings
			)
        {
	        ViewCreator dialogCreator = new( container, DynamicCanvas.DialogsRoot );
	        DialogAggregator = new( dialogCreator, uiSettings.Dialogs );
        }

        private void OnDestroy()
        {
	        DialogAggregator?.Dispose();
        }

        public void ApplyCamera( Camera camera )
        {
            for (int i = 0; i < canvases.Count; i++)
            {
                canvases[i].worldCamera = camera;
            }
        }
	}
}
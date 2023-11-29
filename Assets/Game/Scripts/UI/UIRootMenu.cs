using Game.Systems.StorageSystem;

using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.UI
{
    public class UIRootMenu : MonoBehaviour
    {
        public Camera camera;

        public UIDynamicCanvas dynamicCanvas;

        public List<Canvas> canvases = new();

        [Inject] private GameData gameData;

		private void Awake()
		{
            gameData.IntermediateData.RootMenu = this;
		}

		private void OnDestroy()
		{
            gameData.IntermediateData.RootMenu = null;
		}

		[Button(DirtyOnClick = true)]
        private void ApplyCamera()
        {
            for (int i = 0; i < canvases.Count; i++)
            {
                canvases[i].worldCamera = camera;
            }
        }
    }
}
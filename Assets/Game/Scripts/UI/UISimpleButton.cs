using Game.Systems.StorageSystem;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.UI
{
    public class UISimpleButton : MonoBehaviour
    {
        public Button button;
        public Image frame;
        public Image body;

        [Inject] private GameData gameData; 

        public void Enable(bool trigger)
        {
            var settings = gameData.IntermediateData.GameplayConfig.uiSettings;

			frame.color = trigger ? settings.frame_enable : settings.frame_disable;
			body.color = trigger ? settings.body_enable : settings.body_disable;

            button.interactable = trigger;
		}
	}
}
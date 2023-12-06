using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UILevelStar : MonoBehaviour
    {
        public GameObject on;
		public GameObject off;

        public void Activate(bool trigger)
        {
            on.SetActive(trigger);
			off.SetActive(!trigger);
		}
	}
}
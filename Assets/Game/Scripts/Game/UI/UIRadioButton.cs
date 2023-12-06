using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class UIRadioButton : MonoBehaviour
	{
		public Image on;
		public Image off;

		[Button(DirtyOnClick = true)]
		public void Enable(bool enable)
		{
			on.gameObject.SetActive(enable);
			off.gameObject.SetActive(!enable);
		}
	}
}
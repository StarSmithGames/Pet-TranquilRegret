using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class PreferencesRadioButton : MonoBehaviour
	{
		public Image on;
		public Image off;

		public void Enable(bool enable)
		{
			on.gameObject.SetActive(enable);
			off.gameObject.SetActive(!enable);
		}
	}
}
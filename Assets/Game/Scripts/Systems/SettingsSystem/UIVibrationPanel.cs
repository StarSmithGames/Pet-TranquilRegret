using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.SettingsSystem
{
	public class UIVibrationPanel : MonoBehaviour
	{
		[field: SerializeField] public Image Icon { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Text { get; private set; }
		[field: SerializeField] public Toggle Toggle { get; private set; }
		[Space]
		[SerializeField] protected Sprite on;
		[SerializeField] protected Sprite off;
	}
}
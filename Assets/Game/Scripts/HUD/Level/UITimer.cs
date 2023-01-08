using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.HUD
{
	public class UITimer : MonoBehaviour
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI Time { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI RemainingTime { get; private set; }
		[field: SerializeField] public Slider Slider { get; private set; }
		[field: SerializeField] public Image SliderFill { get; private set; }
		[field: SerializeField] public Image SliderBackground { get; private set; }
		[Space]
		[SerializeField] private SliderColor gold;
		[SerializeField] private SliderColor silver;
		[SerializeField] private SliderColor cooper;

		public void SetRemainigTime()
		{

		}
	}
	[System.Serializable]
	public class SliderColor
	{
		public Color fill;
		public Color background;
	}
}
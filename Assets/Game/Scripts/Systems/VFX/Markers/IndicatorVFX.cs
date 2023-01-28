using Game.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.VFX.Markers
{
	public class IndicatorVFX : MonoBehaviour, IShowable
	{
		public bool IsEnable { get; private set; } = true;
		public bool IsShowing { get; private set; }
		public bool IsInProcess { get; private set; }

		public void Enable(bool trigger)
		{
		}

		public void Show(UnityAction callback = null)
		{
		}

		public void Hide(UnityAction callback = null)
		{
		}
	}
}
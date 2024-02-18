using Game.Systems.LevelSystem;
using UnityEngine;

namespace Game.UI
{
	public sealed class UITimer : MonoBehaviour
	{
		public TMPro.TextMeshProUGUI timeMinutes;
		public TMPro.TextMeshProUGUI timeSeconds;
		public TMPro.TextMeshProUGUI timeMilliseconds;
		[Space]
		public SliderColor gold;
		public SliderColor silver;
		public SliderColor cooper;

		private Timer _levelTimer;

		public void SetTimer( Timer timer )
		{
			_levelTimer = timer;
		}

		private void Update()
		{
			if ( _levelTimer == null ) return;
			
			SetRemainigTime(_levelTimer.GetMinutes(), _levelTimer.GetSeconds(), _levelTimer.GetMilliseconds());
		}

		public void SetRemainigTime(int minutes, int seconds, int milliseconds)
		{
			timeMinutes.text = string.Format("{0:00}", minutes);
			timeSeconds.text = string.Format("{0:00}", seconds);
			timeMilliseconds.text = string.Format("{0:00}", milliseconds);
		}
	}

	[System.Serializable]
	public class SliderColor
	{
		public Color fill;
		public Color background;
	}
}
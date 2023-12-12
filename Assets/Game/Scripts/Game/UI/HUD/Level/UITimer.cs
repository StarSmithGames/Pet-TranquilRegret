using Game.Systems.LevelSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

using Zenject;

namespace Game.HUD.Gameplay
{
	public class UITimer : MonoBehaviour
	{
		public TMPro.TextMeshProUGUI timeMinutes;
		public TMPro.TextMeshProUGUI timeSeconds;
		public TMPro.TextMeshProUGUI timeMilliseconds;
		[Space]
		public SliderColor gold;
		public SliderColor silver;
		public SliderColor cooper;

		private LevelTimer levelTimer;

		[Inject] private LevelManager levelManager;

		private void Start()
		{
			Debug.LogError("Start");

			levelTimer = levelManager.CurrentLevel.LevelTimer;
		}

		private void Update()
		{
			SetRemainigTime(levelTimer.GetMinutes(), levelTimer.GetSeconds(), levelTimer.GetMilliseconds());
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
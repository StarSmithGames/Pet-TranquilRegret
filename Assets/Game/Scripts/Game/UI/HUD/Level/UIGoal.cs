using DG.Tweening;

using Game.Systems.GoalSystem;
using Game.Systems.LevelSystem;

using UnityEngine;
using UnityEngine.UI;

namespace Game.HUD.Gameplay
{
	public class UIGoal : MonoBehaviour
	{
		public Image icon;
		public TMPro.TextMeshProUGUI text;
		public TMPro.TextMeshProUGUI count;
		[Space]
		public PunchData punch;

		private IGoal currentGoal;

		private void OnDestroy()
		{
			if(currentGoal != null)
			{
				currentGoal.onChanged -= UpdateCount;
			}
		}

		public void SetGoal(IGoal goal)
		{
			currentGoal = goal;

			currentGoal.onChanged += UpdateCount;
			UpdateUI();
		}

		private void UpdateUI()
		{
			//icon.sprite = currentGoal.ConfigWrapper.Value.information.portrait;
			//text.text = currentGoal.ConfigWrapper.Value.information.name;

			UpdateCount();
		}

		private void UpdateCount()
		{
			count.text = currentGoal.CurrentValue + " / " + currentGoal.MaxValue;

			Punch();
		}

		public void Punch()
		{
			icon.transform.DORewind();
			count.transform.DORewind();
			icon.transform.DOPunchScale(punch.settings.GetPunch(), punch.settings.duration, punch.settings.vibrato, punch.settings.elasticity);
			count.transform.DOPunchScale(punch.settings.GetPunch(), punch.settings.duration, punch.settings.vibrato, punch.settings.elasticity);
		}
	}
}
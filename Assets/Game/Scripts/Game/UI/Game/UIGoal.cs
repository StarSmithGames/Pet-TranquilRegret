using DG.Tweening;

using Game.Extensions;
using Game.Systems.GoalSystem;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.UI
{
	public sealed class UIGoal : MonoBehaviour
	{
		public Image icon;
		public TMPro.TextMeshProUGUI text;
		public TMPro.TextMeshProUGUI count;
		[Space]
		public PunchData punch;

		private IGoal currentGoal;
		
		public void SetGoal(IGoal goal)
		{
			currentGoal = goal;

			currentGoal.onChanged += UpdateCount;
			UpdateUI();
		}
		
		private void OnDestroy()
		{
			if(currentGoal != null)
			{
				currentGoal.onChanged -= UpdateCount;
			}
		}

		private void UpdateUI()
		{
			icon.sprite = currentGoal.Model.config.information.portrait;
			text.text = currentGoal.Model.config.information.name;

			UpdateCount();
		}

		private void UpdateCount()
		{
			count.text = currentGoal.CurrentValue + " / " + currentGoal.MaxValue;

			Punch();
		}

		public void Punch()
		{
			transform.DORewind();
			transform.DOPunchScale(punch.settings);
		}

		public class Factory : PlaceholderFactory<UIGoal> { }
	}
}
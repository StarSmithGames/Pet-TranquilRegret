using DG.Tweening;

using Game.Managers.LevelManager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

using Zenject;

namespace Game.HUD
{
	public class UIGoal : PoolableObject
	{
		[field: SerializeField] public Image Icon { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Text { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Count { get; private set; }

		[Space]
		[SerializeField] private PunchData punch;

		public CountableGoal CurrentGoal { get; private set; }

		public void SetGoal(CountableGoal goal)
		{
			this.CurrentGoal = goal;

			CurrentGoal.onChanged += UpdateCount;
			UpdateUI();
		}

		private void UpdateUI()
		{
			Icon.sprite = CurrentGoal.Data.information.portrait;
			Text.text = CurrentGoal.Data.information.name;

			UpdateCount();
		}

		private void UpdateCount()
		{
			Count.text = CurrentGoal.Output;

			Punch();
		}

		public void Punch()
		{
			Icon.transform.DORewind();
			Count.transform.DORewind();
			Icon.transform.DOPunchScale(punch.settings.GetPunch(), punch.settings.duration, punch.settings.vibrato, punch.settings.elasticity);
			Count.transform.DOPunchScale(punch.settings.GetPunch(), punch.settings.duration, punch.settings.vibrato, punch.settings.elasticity);
		}

		public class Factory : PlaceholderFactory<UIGoal> { }
	}
}
using Game.Managers.LevelManager;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.HUD
{
	public class UIGoal : PoolableObject
	{
		[field: SerializeField] public Image Icon { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Text { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Count { get; private set; }

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
		}

		public class Factory : PlaceholderFactory<UIGoal> { }
	}
}
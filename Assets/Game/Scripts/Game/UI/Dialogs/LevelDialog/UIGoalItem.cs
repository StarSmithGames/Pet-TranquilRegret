using Game.Systems.GoalSystem;

using StarSmithGames.IoC;

using UnityEngine.UI;

using Zenject;

namespace Game.UI
{
	public class UIGoalItem : PoolableObject
	{
		public Image icon;
		public TMPro.TextMeshProUGUI count;

		public void SetGoal(GoalItemModel item)
		{
			icon.sprite = item.config.information.portrait;
			count.text = item.count.ToString();
		}

		public class Factory : PlaceholderFactory<UIGoalItem> { }
	}
}
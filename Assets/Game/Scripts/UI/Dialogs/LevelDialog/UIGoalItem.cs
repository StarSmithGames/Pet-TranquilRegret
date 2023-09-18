using Game.Systems.LevelSystem;

using StarSmithGames.IoC;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.UI
{
	public class UIGoalItem : PoolableObject
	{
		public Image icon;
		public TMPro.TextMeshProUGUI count;

		public void SetGoal(GoalItem item)
		{
			icon.sprite = item.config.information.portrait;
			count.text = item.count.ToString();
		}

		public class Factory : PlaceholderFactory<UIGoalItem> { }
	}
}
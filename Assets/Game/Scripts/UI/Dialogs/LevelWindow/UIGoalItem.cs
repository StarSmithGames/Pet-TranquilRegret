using StarSmithGames.IoC;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.Managers.LevelManager
{
	public class UIGoalItem : PoolableObject
	{
		[field: SerializeField] public Image Icon { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Count { get; private set; }

		public void SetGoal(CountableGoalConfig goal)
		{
			Icon.sprite = goal.information.portrait;
			Count.text = goal.count.ToString();
		}

		public class Factory : PlaceholderFactory<UIGoalItem> { }
	}
}
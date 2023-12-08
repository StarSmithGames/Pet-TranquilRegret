using Game.Managers.RewardManager;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.UI
{
	public class UIAward : MonoBehaviour
	{
		public Image icon;
		public TMPro.TextMeshProUGUI count;
	
		public void SetAward(AwardItem item)
		{
			icon.sprite = item.data.information.portrait;
			count.text = item.count.ToString();
		}

		public class Factory : PlaceholderFactory<UIAward> { }
	}
}
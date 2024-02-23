using Game.Managers.RewardManager;

using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public sealed class UIAward : MonoBehaviour
	{
		public Image icon;
		public TMPro.TextMeshProUGUI count;
	
		public void SetAward(AwardItem item)
		{
			icon.sprite = item.data.information.portrait;
			count.text = item.count.ToString();
		}
	}
}
using Game.Systems.LevelSystem;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Systems.InfinityRoadSystem
{
#if UNITY_EDITOR
	[ExecuteInEditMode]
#endif
	public class UIRoadLevel : MonoBehaviour
	{
		public event UnityAction<UIRoadLevel> onClicked;

		public bool IsEnable { get; private set; } = true;

		public TMPro.TextMeshProUGUI text;
		public Image background;
		public Image foreground;
		public Button button;
		[Space]
		public LevelType levelType = LevelType.Basic;
		public Color disabledForeground;
		public Color enabledForeground;

#if UNITY_EDITOR
		private void Update()
		{
			if (Application.isPlaying) return;

			text.text = gameObject.name.Split("_")[1];
		}
#endif

		public UIRoadLevel Enable(bool trigger)
		{
			IsEnable = trigger;

			foreground.color = trigger ? enabledForeground : disabledForeground;

			return this;
		}

		/// <param name="count">[0-3]</param>
		//public void EnableStars(int count)
		//{
		//	if(count == 0)
		//	{
		//		star0.gameObject.SetActive(false);
		//		star1.gameObject.SetActive(false);
		//		star2.gameObject.SetActive(false);
		//	}
		//	else if(count == 1)
		//	{
		//		star0.gameObject.SetActive(true);
		//		star1.gameObject.SetActive(false);
		//		star2.gameObject.SetActive(false);
		//	}
		//	else if (count == 2)
		//	{
		//		star0.gameObject.SetActive(true);
		//		star1.gameObject.SetActive(false);
		//		star2.gameObject.SetActive(true);
		//	}
		//	else if (count == 3)
		//	{
		//		star0.gameObject.SetActive(true);
		//		star1.gameObject.SetActive(true);
		//		star2.gameObject.SetActive(true);
		//	}
		//}
		
		public void OnClicked()
		{
			onClicked?.Invoke(this);
		}
	}

	public enum LevelType : int
	{
		Basic	= 0,
		Square	= 1,
	}
}
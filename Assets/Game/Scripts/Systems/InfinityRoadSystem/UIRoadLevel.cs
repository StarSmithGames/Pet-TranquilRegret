using Game.Systems.LevelSystem;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Systems.InfinityRoadSystem
{
	[ExecuteInEditMode]
	public class UIRoadLevel : MonoBehaviour
	{
		public bool IsEnable { get; private set; } = true;

		public event UnityAction<UIRoadLevel> onClicked;

		[SerializeField] private TMPro.TextMeshProUGUI text;
		[SerializeField] private Image background;
		[SerializeField] private Button button;
		[SerializeField] private Image star0;
		[SerializeField] private Image star1;
		[SerializeField] private Image star2;
		[Space]
		[SerializeField] private LevelType levelType = LevelType.Basic;
		[SerializeField] private Color disabled;
		[SerializeField] private Color enabled;
		[SerializeField] private Color passed;

		public LevelConfig levelConfig;

		private void Start()
		{
			button.onClick.AddListener(OnClicked);
		}

		private void OnDestroy()
		{
			button.onClick.RemoveAllListeners();
		}

#if UNITY_EDITOR
		private void Update()
		{
			text.text = gameObject.name.Split("_")[1];
		}
#endif

		public UIRoadLevel SetLevel(LevelConfig config)
		{
			levelConfig = config;

			EnableStars(0);

			return this;
		}

		public UIRoadLevel Enable(bool trigger)
		{
			IsEnable = trigger;

			text.color = trigger ? enabled : disabled;
			background.color = trigger ? enabled : disabled;

			return this;
		}

		/// <param name="count">[0-3]</param>
		public void EnableStars(int count)
		{
			if(count == 0)
			{
				star0.gameObject.SetActive(false);
				star1.gameObject.SetActive(false);
				star2.gameObject.SetActive(false);
			}
			else if(count == 1)
			{
				star0.gameObject.SetActive(true);
				star1.gameObject.SetActive(false);
				star2.gameObject.SetActive(false);
			}
			else if (count == 2)
			{
				star0.gameObject.SetActive(true);
				star1.gameObject.SetActive(false);
				star2.gameObject.SetActive(true);
			}
			else if (count == 3)
			{
				star0.gameObject.SetActive(true);
				star1.gameObject.SetActive(true);
				star2.gameObject.SetActive(true);
			}
		}
		
		private void OnClicked()
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
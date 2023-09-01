using Game.Managers.GameManager;
using Game.Managers.LevelManager;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Zenject;

namespace Game.HUD.Gameplay
{
	public class UITimer : MonoBehaviour
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI TimeMinutes { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI TimeSeconds { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI TimeMilliseconds { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI RemainingTime { get; private set; }
		[field: SerializeField] public Slider Slider { get; private set; }
		[field: SerializeField] public Image SliderFill { get; private set; }
		[field: SerializeField] public Image SliderBackground { get; private set; }
		[field:Space]
		[field: SerializeField] public RectTransform SliderRect { get; private set; }
		[field: SerializeField] public RectTransform Separator0 { get; private set; }
		[field: SerializeField] public RectTransform Separator1 { get; private set; }
		[Space]
		[SerializeField] private SliderColor gold;
		[SerializeField] private SliderColor silver;
		[SerializeField] private SliderColor cooper;

		private bool isPause = true;
		private Timer timer;

		private SignalBus signalBus;

		[Inject]
		private void Construct(SignalBus signalBus)
		{
			this.signalBus = signalBus;
		}

		private void Start()
		{

			signalBus?.Subscribe<SignalGameStateChanged>(OnGameStateChanged);

			//timer = new Timer((float)level.TotalSeconds, true);

			//float width = SliderRect.rect.width;

			//Separator0.anchoredPosition = new Vector2(level.PercentToSilver * (width + 2.5f), Separator0.anchoredPosition.y);
			//Separator1.anchoredPosition = new Vector2(level.PercentToCooper * (width + 2.5f), Separator1.anchoredPosition.y);

			//Slider.value = 0;

			UpdateUI();
		}

		private void OnDestroy()
		{
			signalBus?.TryUnsubscribe<SignalGameStateChanged>(OnGameStateChanged);
		}

		private void Update()
		{
			//if (!isPause)
			//{
			//	timer.Tick();
			//	SetRemainigTime(timer.GetMinutes(), timer.GetSeconds(), timer.GetMilliseconds());

			//	level.SetCurrentSeconds(timer.TotalSeconds);

			//	Slider.value = level.CurrentPercent;

			//	if(level.CurrentReward == LevelReward.Gold)
			//	{
			//		if (Slider.value >= level.PercentToSilver)
			//		{
			//			Separator0.gameObject.SetActive(false);
			//			level.SetCurrentReward(LevelReward.Silver);
			//			UpdateUI();
			//		}
			//	}
			//	else if(level.CurrentReward == LevelReward.Silver)
			//	{
			//		if (Slider.value >= level.PercentToCooper)
			//		{
			//			Separator1.gameObject.SetActive(false);
			//			level.SetCurrentReward(LevelReward.Cooper);
			//			UpdateUI();
			//		}
			//	}
			//}
		}

		public void SetRemainigTime(int minutes, int seconds, int milliseconds)
		{
			TimeMinutes.text = string.Format("{0:00}", minutes);
			TimeSeconds.text = string.Format("{0:00}", seconds);
			TimeMilliseconds.text = string.Format("{0:00}", milliseconds);
		}

		private void UpdateUI()
		{
			//RefreshTime(level.GetEstimateTime());
			//RefreshColor(level.CurrentReward == LevelReward.Gold ? gold : level.CurrentReward == LevelReward.Silver ? silver : cooper);
		}

		//private void RefreshTime(GameTime time)
		//{
		//	RemainingTime.text = string.Format("{0:00}:{1:00}", time.minutes, time.seconds);
		//}

		private void RefreshColor(SliderColor color)
		{
			RemainingTime.color = color.fill;

			SliderFill.color = color.fill;
			SliderBackground.color = color.background;
		}

		private void OnGameStateChanged(SignalGameStateChanged signal)
		{
			if(signal.newGameState == GameState.Gameplay)
			{
			}
		}

		[Button(DirtyOnClick = true)]
		private void CheckGold()
		{
			RefreshColor(gold);
		}

		[Button(DirtyOnClick = true)]
		private void CheckSilver()
		{
			RefreshColor(silver);
		}

		[Button(DirtyOnClick = true)]
		private void CheckCooper()
		{
			RefreshColor(cooper);
		}
	}

	public class Timer
	{
		public event UnityAction onCompleted;

		public float TotalSeconds => t;

		private bool isCompleted = false;
		private float timeRemaining;
		private float t = 0;
		private bool isForward;

		public Timer(float timeRemaining, bool isForward = false)
		{
			this.timeRemaining = timeRemaining;
			this.isForward = isForward;

			t = isForward ? 0 : timeRemaining;
		}

		public void Tick()
		{
			if (isForward)
			{
				t += Time.deltaTime * 5f;

				if (isCompleted) return;

				if (t >= timeRemaining)
				{
					Debug.Log("Time has run out!");
					//t = timeRemaining;

					isCompleted = true;

					onCompleted?.Invoke();
				}
			}
			else
			{
				if (isCompleted) return;

				if (t > 0)
				{
					t -= Time.deltaTime;
				}
				else
				{
					Debug.Log("Time has run out!");
					t = 0;

					isCompleted = true;

					onCompleted?.Invoke();
				}
			}
		}

		public int GetMinutes()
		{
			return Mathf.FloorToInt(t / 60);
		}

		public int GetSeconds()
		{
			return Mathf.FloorToInt(t % 60);
		}

		public int GetMilliseconds()
		{
			return Mathf.FloorToInt((t % 1) * 100);
		}
	}

	[System.Serializable]
	public class SliderColor
	{
		public Color fill;
		public Color background;

	}
}
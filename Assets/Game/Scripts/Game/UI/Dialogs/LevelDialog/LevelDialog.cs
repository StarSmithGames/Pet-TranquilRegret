using Game.Systems.GameSystem;
using Game.Systems.LevelSystem;
using Game.Systems.StorageSystem;

using StarSmithGames.Core;
using StarSmithGames.Go.LocalizationSystem;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.UI
{
	public class LevelDialog : UIViewDialog
	{
		public event Action onStartClicked;
		public event Action onLeftClicked;
		public event Action onRightClicked;
		public event Action onClosed;

		public TMPro.TextMeshProUGUI title;
		public Transform goalContent;
		public Button startButton;
		public Button leftButton;
		public Button rightButton;
		public Button closeButton;
		[Space]
		public List<UILevelStar> stars = new();

		private List<UIGoalItem> goals = new List<UIGoalItem>();
		private LevelConfig levelConfig;
		private RegularLevelData levelData;

		[Inject] private GameService _gameService;
		[Inject] private LocalizationSystem localizationSystem;
		[Inject] private UIGoalItem.Factory goalFactory;

		private void Awake()
		{
			goalContent.DestroyChildren();
		}

		public override void Show(Action callback = null)
		{
			leftButton.gameObject.SetActive(false);
			rightButton.gameObject.SetActive(false);

			title.text = $"{localizationSystem.Translate("ui.level_dialog.title")} {levelConfig.id}";

			for (int i = 0; i < stars.Count; i++)
			{
				stars[i].Activate((i + 1) <= levelData.stars);
			}

			RerfreshGoals();

			Interactable(true);

			base.Show(callback);
		}

		public void SetLevel(LevelConfig levelConfig, RegularLevelData levelData)
		{
			this.levelConfig = levelConfig;
			this.levelData = levelData;
		}

		private void Interactable(bool trigger)
		{
			startButton.interactable = trigger;
			leftButton.interactable = trigger;
			rightButton.interactable = trigger;
			closeButton.interactable = trigger;
		}

		private void RerfreshGoals()
		{
			goals.Clear();
			goalContent.DestroyChildren();

			if (levelConfig.primaryGoals.Count == 0) return;

			for (int i = 0; i < levelConfig.primaryGoals.Count; i++)
			{
				var goal = goalFactory.Create();
				goal.transform.SetParent(goalContent);
				goal.transform.localScale = Vector3.one;
				goal.transform.localPosition = Vector3.zero;

				goal.SetGoal(levelConfig.primaryGoals[i]);

				goals.Add(goal);
			}
		}

		public void OnStartClick()
		{
			Interactable(false);

			_gameService.StartRegularLevel(levelConfig);

			onStartClicked?.Invoke();
		}

		public void OnLeftClick()
		{
			//Interactable(false);
		}

		public void OnRightClick()
		{
			//Interactable(false);
		}

		public void OnBackClick()
		{
			Interactable(false);

			HideAndDispose();

			onClosed?.Invoke();
		}
	}
}
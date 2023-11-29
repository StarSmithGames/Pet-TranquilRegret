using Game.Services;
using Game.Systems.GameSystem;
using Game.Systems.LevelSystem;

using StarSmithGames.Core;
using StarSmithGames.Go;
using StarSmithGames.Go.LocalizationSystem;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.UI
{
	public class LevelDialog : ViewPopupBase
	{
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

		[Inject] private GameLoader gameLoader;
		[Inject] private ViewService viewService;
		private LocalizationSystem localizationSystem;
		//private UIGoalItem.Factory goalFactory;
		//UIGoalItem.Factory goalFactory,
		[Inject]
		private void Construct(
			LocalizationSystem localizationSystem)
		{
			this.localizationSystem = localizationSystem;
		}

		private void Awake()
		{
			goalContent.DestroyChildren();
		}

		private void OnDestroy()
		{
			viewService.ViewDialogRegistrator.UnRegistrate(this);
		}

		public override void Show(Action callback = null)
		{
			leftButton.gameObject.SetActive(false);
			rightButton.gameObject.SetActive(false);

			for (int i = 0; i < stars.Count; i++)
			{
				stars[i].Activate(false);
			}

			Interactable(true);

			base.Show(callback);
		}

		public void SetLevel(LevelConfig levelConfig)
		{
			this.levelConfig = levelConfig;

			title.text = $"{localizationSystem.Translate("ui.level_dialog.title")} {levelConfig.id}";

			goals.Clear();
			goalContent.DestroyChildren();

			for (int i = 0; i < levelConfig.primaryGoals.Count; i++)
			{
				//var goal = goalFactory.Create();
				//goal.transform.SetParent(goalContent);
				//goal.transform.localScale = Vector3.one;
				//goal.transform.localPosition = Vector3.zero;

				//goal.SetGoal(levelConfig.primaryGoals[i]);

				//goals.Add(goal);
			}
		}

		private void Interactable(bool trigger)
		{
			startButton.interactable = trigger;
			leftButton.interactable = trigger;
			rightButton.interactable = trigger;
			closeButton.interactable = trigger;
		}

		public void OnStartClick()
		{
			Interactable(false);

			gameLoader.LoadLevel(levelConfig);
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

			Hide();
		}
	}
}
using Game.Managers.LevelManager;
using Game.Systems.GameSystem;
using Game.UI;

using StarSmithGames.Core;
using StarSmithGames.Go;
using StarSmithGames.Go.LocalizationSystem;
using StarSmithGames.Go.SceneManager;

using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

using Zenject;

namespace Game.UI
{
	public class LevelDialog : ViewPopupBase
	{
		public TMPro.TextMeshProUGUI title;
		public Transform goalContent;
		public Button startButton;

		private List<UIGoalItem> goals = new List<UIGoalItem>();
		private LevelConfig levelConfig;

		private UIMenuCanvas menuCanvas;
		private UIGoalItem.Factory goalFactory;
		private SceneManager sceneManager;
		private LocalizationSystem localizationSystem;

		[Inject]
		private void Construct(UIMenuCanvas menuCanvas,
			UIGoalItem.Factory goalFactory,
			SceneManager sceneManager,
			LocalizationSystem localizationSystem)
		{
			this.menuCanvas = menuCanvas;
			this.goalFactory = goalFactory;
			this.sceneManager = sceneManager;
			this.localizationSystem = localizationSystem;
		}

		private void Awake()
		{
			menuCanvas.ViewRegistrator.Registrate(this);
			goalContent.DestroyChildren();
		}

		private void OnDestroy()
		{
			menuCanvas.ViewRegistrator.UnRegistrate(this);
		}

		public override void Show(Action callback = null)
		{
			startButton.interactable = true;

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
			startButton.interactable = false;

			var name = Path.GetFileNameWithoutExtension(levelConfig.scene.ScenePath);
			Debug.LogError(name);
			Debug.LogError(levelConfig.scene.ScenePath);
			sceneManager.LoadSceneAsyncFromAddressables(name, levelConfig.scene.ScenePath);
		}

		public void OnBackClick()
		{
			Hide();
		}
	}
}
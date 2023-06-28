using Game.Managers.LevelManager;
using Game.Systems.GameSystem;
using Game.UI;

using StarSmithGames.Core;
using StarSmithGames.Go;
using StarSmithGames.Go.SceneManager;

using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

using Zenject;

namespace Game.HUD.Menu
{
	public class LevelWindow : ViewPopupBase
	{
		public TMPro.TextMeshProUGUI title;
		public Transform goalContent;
		public Button startButton;
		public Button blank;

		private List<UIGoalItem> goals = new List<UIGoalItem>();
		private LevelConfig levelConfig;

		private UIMenuCanvas menuCanvas;
		private UIGoalItem.Factory goalFactory;
		private SceneManager sceneManager;

		[Inject]
		private void Construct(UIMenuCanvas menuCanvas, UIGoalItem.Factory goalFactory, SceneManager sceneManager)
		{
			this.menuCanvas = menuCanvas;
			this.goalFactory = goalFactory;
			this.sceneManager = sceneManager;
		}

		private void Start()
		{
			goalContent.DestroyChildren();

			startButton.onClick.AddListener(OnStart);
			blank.onClick.AddListener(OnClose);
			
			menuCanvas.ViewRegistrator.Registrate(this);
		}

		private void OnDestroy()
		{
			startButton.onClick.RemoveAllListeners();
			blank.onClick.RemoveAllListeners();

			menuCanvas.ViewRegistrator.UnRegistrate(this);
		}

		public void SetLevel(LevelConfig levelConfig)
		{
			this.levelConfig = levelConfig;

			title.text = $"{levelConfig.name}";

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

		private void OnStart()
		{
			var name = Path.GetFileNameWithoutExtension(levelConfig.scene.ScenePath);
			Debug.LogError(name);
			Debug.LogError(levelConfig.scene.ScenePath);
			sceneManager.LoadSceneAsyncFromAddressables(name, levelConfig.scene.ScenePath);
		}

		private void OnClose()
		{
			Hide();
		}
	}
}
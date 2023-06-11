using Game.Managers.LevelManager;
using Game.UI;

using StarSmithGames.Core;
using StarSmithGames.Go;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.HUD.Menu
{
	public class LevelWindow : ViewPopupBase
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI Title { get; private set; }
		[field: SerializeField] public Transform GoalContent { get; private set; }
		[field: SerializeField] public Button StartButton { get; private set; }
		[field: SerializeField] public Button Blank { get; private set; }

		private List<UIGoalItem> goals = new List<UIGoalItem>();
		private LevelSettings settings;

		private UIMenuCanvas menuCanvas;
		private UIGoalItem.Factory goalFactory;
		//private SceneManager sceneManager;

		[Inject]
		private void Construct(UIMenuCanvas menuCanvas, UIGoalItem.Factory goalFactory/*, SceneManager sceneManager*/)
		{
			this.menuCanvas = menuCanvas;
			this.goalFactory = goalFactory;
			//this.sceneManager = sceneManager;
		}

		private void Start()
		{
			StartButton.onClick.AddListener(OnStart);
			Blank.onClick.AddListener(OnClose);

			GoalContent.DestroyChildren();

			menuCanvas.ViewRegistrator.Registrate(this);
		}

		private void OnDestroy()
		{
			StartButton.onClick.RemoveAllListeners();
			Blank.onClick.RemoveAllListeners();
			menuCanvas.ViewRegistrator.UnRegistrate(this);
		}

		public void SetLevel(LevelSettings settings)
		{
			this.settings = settings;

			Title.text = $"Level {settings.name.Split("_")[1]}";

			StarSmithGames.Core.CollectionExtensions.Resize(settings.primaryGoals, goals,
			() =>
			{
				var goal = goalFactory.Create();
				goal.transform.SetParent(GoalContent);
				goal.transform.localScale = Vector3.one;
				goal.transform.localPosition = Vector3.zero;

				return goal;
			},
			() =>
			{
				var goal = goals.Last();
				goal.DespawnIt();

				return goal;
			});

			for (int i = 0; i < goals.Count; i++)
			{
				goals[i].SetGoal(settings.primaryGoals[i]);
			}
		}

		private void OnStart()
		{
			//sceneManager.SwitchScene(settings.name, false);
		}

		private void OnClose()
		{
			Hide();
		}
	}
}
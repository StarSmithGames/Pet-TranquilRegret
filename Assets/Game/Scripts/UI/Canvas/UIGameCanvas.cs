using Game.HUD.Gameplay;
using Game.Managers.LevelManager;
using Game.Systems.NavigationSystem;

using StarSmithGames.Core;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;

namespace Game.UI
{
    public class UIGameCanvas : UISubCanvas
    {
		[field: SerializeField] public Transform GoalContent { get; private set; }
		[field: Header("Control")]
		[field: SerializeField] public UIJoystick Joystick { get; private set; }
		[field: SerializeField] public UIDropButton Drop { get; private set; }


		private List<UIGoal> goals = new List<UIGoal>();

		private SignalBus signalBus;
		private LevelManager levelManager;
		private UIGoal.Factory goalFactory;

		[Inject]
		private void Construct(SignalBus signalBus, LevelManager levelManager, UIGoal.Factory goalFactory)
		{
			this.signalBus = signalBus;
			this.levelManager = levelManager;
			this.goalFactory = goalFactory;
		}

		private void Start()
		{
			GoalContent.DestroyChildren();

			StarSmithGames.Core.CollectionExtensions.Resize(levelManager.CurrentLevel.PrimaryGoals, goals,
			() =>
			{
				var goal = goalFactory.Create();
				goal.transform.SetParent(GoalContent);
				goal.transform.localScale = Vector3.one;
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
				goals[i].SetGoal(levelManager.CurrentLevel.PrimaryGoals[i]);
			}
		}
	}
}
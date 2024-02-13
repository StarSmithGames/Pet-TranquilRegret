using Game.HUD.Gameplay;
using Game.Systems.LevelSystem;
using Game.Systems.NavigationSystem;

using StarSmithGames.Core;

using System.Collections.Generic;

using UnityEngine;

using Zenject;

namespace Game.UI
{
	public class UIGameCanvas : MonoBehaviour
	{
		public Transform dialogsRoot;
		[Space]
		public Transform goalContent;
		[Header("Control")]
		public UIJoystick joystick;
		public HUDDropButton drop;
		public HUDJumpButton jump;

		private List<UIGoal> goals = new List<UIGoal>();

		[Inject] private UIGoal.Factory goalFactory;
		[Inject] private LevelManager levelManager;

		private void Start()
		{
			///TODO REMOVE!
			var registrator = levelManager.CurrentLevel.Presenter.Gameplay.GoalRegistrator;
			
			goals.Clear();
			goalContent.DestroyChildren(true);
			
			registrator.GoalsPrimary.ForEach((x) =>
			{
				var goal = goalFactory.Create();
				goal.transform.SetParent(goalContent);
				goal.transform.localScale = Vector3.one;
				goal.SetGoal(x);

				goals.Add(goal);
			});
		}
	}
}
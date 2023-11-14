using Game.HUD.Gameplay;
using Game.Managers.GameManager;
using Game.Systems.NavigationSystem;
using Game.Systems.StorageSystem;

using StarSmithGames.Core;

using System.Collections.Generic;

using UnityEngine;

using Zenject;

namespace Game.UI
{
	public class UIGameCanvas : UICanvas
    {
		[Space]
		public Transform goalContent;
		public UIGoal goalPrefab;
		[Header("Control")]
		public UIJoystick joystick;
		public HUDDropButton drop;
		public HUDJumpButton jump;

		private List<UIGoal> goals = new List<UIGoal>();

		[Inject] private GameData gameData;
		private SignalBus signalBus;

		[Inject]
		private void Construct(SignalBus signalBus)
		{
			this.signalBus = signalBus;

			signalBus.Subscribe<SignalGameStateChanged>(OnGameStateChanged);
		}

		private void OnDestroy()
		{
			signalBus.Unsubscribe<SignalGameStateChanged>(OnGameStateChanged);
		}

		private void OnGameStateChanged(SignalGameStateChanged signal)
		{
			if(signal.newGameState == GameState.PreGameplay)
			{
				var registrator = gameData.IntermediateData.LevelPresenter.Model.GoalRegistrator;

				goalContent.DestroyChildren();
				registrator.GoalsPrimary.ForEach((x) =>
				{
					var goal = SceneContext.Instantiate(goalPrefab);
					goal.transform.SetParent(goalContent);
					goal.transform.localScale = Vector3.one;
					goal.SetGoal(x);
				});
			}
		}
	}
}
using Game.Managers.CharacterManager;
using Game.Systems.LevelSystem;
using Game.Systems.UISystem;
using StarSmithGames.Core;
using System.Collections.Generic;

using UnityEngine;

using Zenject;

namespace Game.UI
{
	public sealed class UIGameCanvas : UICanvas
	{
		public Transform goalContent;
		public UITimer Timer;
		
		private List<UIGoal> _goals = new();

		private UIGoal.Factory _goalFactory;
		private UIRootGame _uiRootGame;
		
		[ Inject ]
		private void Construct(
			CharacterManager characterManager,
			UIGoal.Factory goalFactory,
			UIRootGame uiRootGame
			)
		{
			_goalFactory = goalFactory;
			_uiRootGame = uiRootGame;
		}
		
		public void SetLevel( LevelPresenter presenter )
		{
			Timer.SetTimer( presenter.Timer );
			
			_goals.Clear();
			goalContent.DestroyChildren(true);
			
			var targets = presenter.Gameplay.GoalRegistrator.GoalsPrimary;
			targets.ForEach((x) =>
			{
				var goal = _goalFactory.Create();
				goal.transform.SetParent(goalContent);
				goal.transform.localScale = Vector3.one;
				goal.SetGoal(x);

				_goals.Add(goal);
			});
		}
		
		public void OnSettingsButtonClick()
		{
			_uiRootGame.DialogAggregator.ShowAndCreateIfNotExist< SettingsDialog >();
		}
	}
}
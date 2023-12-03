using Cysharp.Threading.Tasks;

using Game.Systems.FloatingSystem;
using Game.Systems.GoalSystem;
using Game.Systems.InteractionSystem;
using Game.Systems.NavigationSystem;
using Game.Systems.StorageSystem;
using Game.Systems.ZoneSystem;

using ModestTree;

using Sirenix.OdinInspector;

using StarSmithGames.Core;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using UnityEngine;
using UnityEngine.Playables;

using Zenject;

using static UnityEditor.Progress;

namespace Game.Systems.LevelSystem
{
	public class UncoveredContainer : InteractableContainer
	{
		public CharacterInteractionZone interactionZone;
		public Settings settings;

		private Character.Character currentTarget;
		private CancellationTokenSource floatCancellation;

		[Inject] private GameData gameData;
		[Inject] private FloatingSystem.FloatingSystem floatingSystem;

		private void Awake()
		{
			if (settings.selectionType == SelectionType.Random)
			{
				items = items.Shuffle();
			}
			else if(settings.selectionType == SelectionType.Backward)
			{
				items.Reverse();
			}

			Subsctibe();
		}

		private void OnDestroy()
		{
			Unsubscribe();
		}

		private void Subsctibe()
		{
			interactionZone.onItemAdded += OnCharacterAdded;
			interactionZone.onItemRemoved += OnCharacterRemoved;
		}

		private void Unsubscribe()
		{
			interactionZone.onItemAdded -= OnCharacterAdded;
			interactionZone.onItemRemoved -= OnCharacterRemoved;
		}

		private async UniTask DoFloatAsync()
		{
			floatCancellation?.Dispose();
			floatCancellation = new();
			await FloatAsync(floatCancellation.Token);
		}

		private void BreakFloat()
		{
			floatCancellation?.Cancel();
			floatCancellation?.Dispose();
			floatCancellation = null;
		}

		private async UniTask FloatAsync(CancellationToken cancellation)
		{
			while(items.Count != 0 && !cancellation.IsCancellationRequested)
			{
				var item = items.First();
				items.Remove(item);

				//Animation
				if (settings.isOneByOne)
					await item.GetComponent<FloatingComponent>().DoAnimationAsync(currentTarget.transform);
				else
					item.GetComponent<FloatingComponent>().DoAnimationAsync(currentTarget.transform);

				if (cancellation.IsCancellationRequested)
				{
					//Interrupted
					//Debug.LogError("Items " + items.Count);
				}

				//Waiter
				if(settings.waitBetween == 0)
					await UniTask.Yield(cancellationToken: cancellation);
				else
					await UniTask.WaitForSeconds(settings.waitBetween, cancellationToken: cancellation);
			}

			BreakFloat();
			Dispose();
		}

		private void Dispose()
		{
			Unsubscribe();
			currentTarget = null;
		}

		private void OnCharacterAdded(Character.Character character)
		{
			if (currentTarget != null) return;

			currentTarget = character;

			DoFloatAsync();
		}

		private void OnCharacterRemoved(Character.Character character)
		{
			Assert.IsNotNull(currentTarget);
		
			BreakFloat();
			currentTarget = null;
		}

		private void OnAnimationCompleted(FloatingComponent obj)
		{
			//if(settings.floatingType == SelectionType.All)
			{
				//if (obj is GoalModel model)
				//{
				//	int totalCount = 0;

				//	for (int i = 0; i < floatingObjects.Count; i++)
				//	{
				//		totalCount += (floatingObjects[i] as GoalModel).goal.count;
				//	}

				//	floatingSystem.CreateText(obj.CurrentTarget.position, $"+{totalCount}", model.goal.config.information.portrait);

				//	gameData.IntermediateData.Level.GoalRegistrator.AccumulatePrimaryGoals(model.goal);

				//	model.Goal.CurrentValue += totalCount;
				//}
				//else if (obj is Coin coin)
				//{
				//	int totalCount = 0;

				//	for (int i = 0; i < floatingObjects.Count; i++)
				//	{
				//		totalCount += (floatingObjects[i] as Coin).Count;
				//	}

				//	floatingSystem.CreateText(obj.CurrentTarget.position, $"+{totalCount}", color: Color.yellow);
				//	//levelManager.CurrentLevel.Coins.CurrentValue += totalCount;
				//}
			}
			//else
			{
				//if (obj is GoalModel model)
				//{
				//	floatingSystem.CreateText(obj.CurrentTarget.position, $"+{model.goal.count}", model.goal.config.information.portrait);
				//	gameData.IntermediateData.LevelPresenter.Model.GoalRegistrator.AccumulatePrimaryGoal(model.goal);
				//}
				//else if (obj is Coin coin)
				//{
				//	floatingSystem.CreateText(obj.CurrentTarget.position, $"+{coin.Count}", color: Color.yellow);
				//	//levelManager.CurrentLevel.Coins.CurrentValue += coin.Count;
				//}
			}
		}


		[System.Serializable]
		public class Settings
		{
			public SelectionType selectionType = SelectionType.Forward;
			public bool isOneByOne = false;
			public float waitBetween = 0.25f;
		}
	}

	public enum SelectionType
	{
		Forward,
		Backward,
		Random,
	}
}
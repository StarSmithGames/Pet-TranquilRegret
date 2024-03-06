using Cysharp.Threading.Tasks;

using Game.Systems.FloatingSystem;
using Game.Systems.LevelSystem;
using Game.Systems.NavigationSystem;
using Game.Systems.StorageSystem;

using ModestTree;

using Sirenix.OdinInspector;

using StarSmithGames.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Unity.VisualScripting;

using UnityEngine;

using Zenject;

namespace Game.Systems.InventorySystem
{
	[AddComponentMenu("AGame/Inventory/Uncovered Container")]
	public partial class UncoveredContainer : ContainerView
	{
		public List<ItemView> itemViews = new();

		public CharacterInteractionZone interactionZone;
		public Settings settings;

		private Entity.CharacterSystem.Character currentTarget;
		private ItemViewFloating floating;

		[Inject] private LevelManager levelManager;

		[Inject]
		private void Construct()
		{
			if (settings.selectionType == SelectionType.Random)
			{
				itemViews = itemViews.Shuffle();
			}
			else if (settings.selectionType == SelectionType.Backward)
			{
				itemViews.Reverse();
			}

			Initialize();

			floating = new(this);

			Subsctibe();
		}

		private void OnDestroy()
		{
			Dispose();
		}

		private void Dispose()
		{
			Unsubscribe();
			currentTarget = null;
		}

		public override ItemModel[] GetItems()
		{
			return itemViews.Select((x) => x.model).ToArray();
		}

		private void OnCharacterAdded(Entity.CharacterSystem.Character character)
		{
			if (currentTarget != null) return;

			currentTarget = character;

			_ = floating.DoFloatAsync(currentTarget.transform,
			(item) =>
			{
				var goal = item.model.config as GoalItemConfig;
				levelManager.CurrentLevel.Presenter.Gameplay.GoalRegistrator.AccumulatePrimaryGoal(goal);
			},
			() => Dispose());
		}

		private void OnCharacterRemoved(Entity.CharacterSystem.Character character)
		{
			Assert.IsNotNull(currentTarget);

			floating.BreakFloat();
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

#if UNITY_EDITOR
		[Button(DirtyOnClick = true)]
		private void Fill()
		{
			itemViews = GetComponentsInChildren<ItemView>().ToList();
		}
#endif

		[System.Serializable]
		public class Settings
		{
			public SelectionType selectionType = SelectionType.Random;
			public bool isOneByOne = false;
			public float waitBetween = 0.16f;
		}
	}

	public partial class UncoveredContainer
	{
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
	}



	public sealed class ItemViewFloating
	{
		private CancellationTokenSource floatCancellation;

		private Transform target;

		private UncoveredContainer container;

		public ItemViewFloating(
			UncoveredContainer container
			)
		{
			this.container = container;
		}

		public async UniTask DoFloatAsync(Transform target, Action<ItemView> onItemPopped = null, Action callback = null)
		{
			this.target = target;

			floatCancellation?.Dispose();
			floatCancellation = new();
			await FloatAsync(floatCancellation.Token, onItemPopped, callback);
		}

		public void BreakFloat()
		{
			floatCancellation?.Cancel();
			floatCancellation?.Dispose();
			floatCancellation = null;
		}

		private async UniTask FloatAsync(CancellationToken cancellation, Action<ItemView> onItemPopped = null, Action callback = null)
		{
			while (container.itemViews.Count != 0 && !cancellation.IsCancellationRequested)
			{
				var item = Pop();

				onItemPopped?.Invoke(item);

				//Animation
				if (container.settings.isOneByOne)
					await item.GetComponent<FloatingComponent>().DoAnimationAsync(target);
				else
					item.GetComponent<FloatingComponent>().DoAnimationAsync(target);

				if (cancellation.IsCancellationRequested)
				{
					//Interrupted
					//Debug.LogError("Items " + items.Count);
				}

				//Waiter
				if (container.settings.waitBetween == 0)
					await UniTask.Yield(cancellationToken: cancellation);
				else
					await UniTask.WaitForSeconds(container.settings.waitBetween, cancellationToken: cancellation);
			}

			BreakFloat();
			callback?.Invoke();
		}


		private ItemView Pop()
		{
			var item = container.itemViews.First();
			container.itemViews.Remove(item);
			container.Model.Inventory.UnRegistrate(item.model);

			return item;
		}
	}

	public enum SelectionType
	{
		Forward,
		Backward,
		Random,
	}
}
using Game.Systems.FloatingSystem;
using Game.Systems.NavigationSystem;
using Game.Systems.StorageSystem;

using Sirenix.OdinInspector;

using StarSmithGames.Core;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Playables;

using Zenject;

namespace Game.Systems.LevelSystem
{
	[RequireComponent(typeof(InteractionZone))]
	public class AccessibleContainer : MonoBehaviour
	{
		public InteractionZone interactionZone;
		public List<Floating3DObject> floatingObjects = new List<Floating3DObject>();
		public Settings settings;

		private Transform currentTarget;

		[Inject] private GameData gameData;
		[Inject] private FloatingSystem.FloatingSystem floatingSystem;

		private void Start()
		{
			if (floatingObjects.Count > 0)
			{
				if (settings.type == FloatingType.Random)
				{
					floatingObjects = floatingObjects.Shuffle();
				}
				StartCoroutine(Observable());
			}

			interactionZone.Registrator.onCollectionChanged += OnZoneCollectionChanged;
		}

		private void OnDestroy()
		{
			interactionZone.Registrator.onCollectionChanged -= OnZoneCollectionChanged;
		}

		private IEnumerator Observable()
		{
			while (true)
			{
				if (currentTarget != null)
				{
					if (settings.type == FloatingType.All)
					{
						for (int i = 0; i < floatingObjects.Count; i++)
						{
							floatingObjects[i].SetTarget(currentTarget, i == floatingObjects.Count - 1 ? OnAnimationCompleted : null);
						}
						yield break;
					}
					else
					{
						var item = GetItem();

						if (item == null)
						{
							yield break;
						}

						item.SetTarget(currentTarget, OnAnimationCompleted);
						yield return new WaitForSeconds(settings.waitBetween);
					}
				}

				yield return null;
			}
		}

		private Floating3DObject GetItem()
		{
			Floating3DObject obj = null;

			if(settings.type == FloatingType.Forward || settings.type == FloatingType.Random)
			{
				obj = floatingObjects.Find((x) => !x.IsHasTarget);
			}
			else if(settings.type == FloatingType.Backward)
			{
				obj = floatingObjects.FindLast((x) => !x.IsHasTarget);
			}

			return obj;
		}

		private void OnZoneCollectionChanged()
		{
			currentTarget = interactionZone.Registrator.registers.FirstOrDefault()?.transform;
		}

		private void OnAnimationCompleted(Floating3DObject obj)
		{
			if(settings.type == FloatingType.All)
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
			else
			{
				if (obj is GoalModel model)
				{
					floatingSystem.CreateText(obj.CurrentTarget.position, $"+{model.goal.count}", model.goal.config.information.portrait);
					gameData.IntermediateData.Level.GoalRegistrator.AccumulatePrimaryGoal(model.goal);
				}
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
			floatingObjects = GetComponentsInChildren<Floating3DObject>().ToList();
		}
#endif

		[System.Serializable]
		public class Settings
		{
			public FloatingType type = FloatingType.Forward;
			[ShowIf("@type != FloatingType.All")]
			public float waitBetween = 0.25f;
		}
	}

	public enum FloatingType
	{
		Forward,
		Backward,
		Random,
		All,
	}
}
using Game.Entities;
using Game.Managers.LevelManager;
using Game.Systems.InteractionSystem;

using Sirenix.OdinInspector;

using StarSmithGames.Core;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;

namespace Game.Systems.FloatingSystem
{
	[RequireComponent(typeof(InteractionZone))]
	public class LightContainer : MonoBehaviour
	{
		[SerializeField] private InteractionZone interactionZone;
		[SerializeField] private List<Floating3DObject> floatingObjects = new List<Floating3DObject>();
		[SerializeField] private Settings settings;

		private Transform currentTarget;

		private FloatingSystem floatingSystem;

		[Inject]
		private void Construct(FloatingSystem floatingSystem)
		{
			this.floatingSystem = floatingSystem;

		}

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

			interactionZone.onCollectionChanged += OnZoneCollectionChanged;
		}

		private void OnDestroy()
		{
			interactionZone.onCollectionChanged -= OnZoneCollectionChanged;
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
						var obj = GetObject();

						if (obj == null)
						{
							yield break;
						}

						obj.SetTarget(currentTarget, OnAnimationCompleted);
						yield return new WaitForSeconds(settings.waitBetween);
					}
				}

				yield return null;
			}
		}

		private Floating3DObject GetObject()
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
			currentTarget = interactionZone.GetCollection().FirstOrDefault()?.transform;
		}

		private void OnAnimationCompleted(Floating3DObject obj)
		{
			if(settings.type == FloatingType.All)
			{
				if (obj is GoalCountable countable)
				{
					int totalCount = 0;

					for (int i = 0; i < floatingObjects.Count; i++)
					{
						totalCount += (floatingObjects[i] as GoalCountable).Count;
					}

					floatingSystem.CreateText(obj.CurrentTarget.position, $"+{totalCount}", countable.Data.information.portrait);
					countable.Goal.CurrentValue += totalCount;
				}
				else if (obj is Coin coin)
				{
					int totalCount = 0;

					for (int i = 0; i < floatingObjects.Count; i++)
					{
						totalCount += (floatingObjects[i] as Coin).Count;
					}

					floatingSystem.CreateText(obj.CurrentTarget.position, $"+{totalCount}", color: Color.yellow);
					//levelManager.CurrentLevel.Coins.CurrentValue += totalCount;
				}
			}
			else
			{
				if (obj is GoalCountable countable)
				{
					floatingSystem.CreateText(obj.CurrentTarget.position, $"+{countable.Count}", countable.Data.information.portrait);
					countable.Goal.CurrentValue += countable.Count;
				}
				else if (obj is Coin coin)
				{
					floatingSystem.CreateText(obj.CurrentTarget.position, $"+{coin.Count}", color: Color.yellow);
					//levelManager.CurrentLevel.Coins.CurrentValue += coin.Count;
				}
			}
		}

		[Button(DirtyOnClick = true)]
		private void Fill()
		{
			floatingObjects = GetComponentsInChildren<Floating3DObject>().ToList();
		}

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
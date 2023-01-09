using Game.Systems.InteractionSystem;

using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Game.Systems.FloatingSystem
{
	[RequireComponent(typeof(InteractionPoint))]
	public class FloatingContainer : MonoBehaviour
	{
		[SerializeField] private LayerMask layer;
		[SerializeField] private InteractionPoint interactionPoint;
		[SerializeField] private List<Floating3DObject> floatingObjects = new List<Floating3DObject>();
		[SerializeField] private Settings settings;

		private Transform target;

		private void Start()
		{
			if(settings.type == FloatingType.Random)
			{
				floatingObjects.Shuffle();
			}

			if (floatingObjects.Count > 0)
			{
				StartCoroutine(Observable());
			}
		}

		private IEnumerator Observable()
		{
			while (true)
			{
				if (IsAnyoneInRange(out Collider[] colliders))
				{
					target = colliders.First().transform;
				}
				else
				{
					target = null;
				}

				if (target != null)
				{
					if (settings.type == FloatingType.All)
					{
						for (int i = 0; i < floatingObjects.Count; i++)
						{
							floatingObjects[i].SetTarget(target);
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

						obj.SetTarget(target);
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
				obj = floatingObjects.First((x) => !x.IsHasTarget);
			}
			else if(settings.type == FloatingType.Backward)
			{
				obj = floatingObjects.Last((x) => !x.IsHasTarget);
			}

			return obj;
		}

		private bool IsAnyoneInRange(out Collider[] colliders)
		{
			colliders = Physics.OverlapSphere(transform.position, interactionPoint.InteractionSettings.maxRange, layer);

			return colliders.Length > 0;
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
using Game.Systems.InteractionSystem;

using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Game.Systems.FloatingSystem
{
	public class FloatingContainer : MonoBehaviour
	{
		[SerializeField] private LayerMask layer;
		[SerializeField] private InteractionPoint interactionPoint;
		[SerializeField] private List<Floating3DObject> floatingObjects = new List<Floating3DObject>();

		private Transform target;

		private void Start()
		{
			StartCoroutine(Observable());
		}

		private IEnumerator Observable()
		{
			while (true)
			{
				if (floatingObjects.Count == 0)
				{
					yield break;
				}

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
					Floating3DObject obj = null;

					for (int i = 0; i < floatingObjects.Count; i++)
					{
						if (!floatingObjects[i].IsHasTarget)
						{
							obj = floatingObjects[i];
							break;
						}
					}

					if(obj == null)
					{
						yield break;
					}

					obj.SetTarget(target);
					yield return new WaitForSeconds(0.25f);
				}

				yield return null;
			}
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
	}
}
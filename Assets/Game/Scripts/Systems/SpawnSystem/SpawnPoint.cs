using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.SpawnSystem
{
	public class SpawnPoint : MonoBehaviour
	{
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position, 0.25f);
		}
	}
}
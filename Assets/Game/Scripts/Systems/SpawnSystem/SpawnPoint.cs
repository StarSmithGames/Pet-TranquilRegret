using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Systems.SpawnSystem
{
	public class SpawnPoint : MonoBehaviour
	{
		[OnValueChanged("OnChanged", true)]
		[SerializeField] private Settings settings;

		public Vector3 GetFollowPosition()
		{
			return transform.position + settings.followPoint;
		}

		public Vector3 GetTracketObjectOffset()
		{
			return settings.tracketObjectOffset;
		}

		public Vector3 GetLookAtPosition()
		{
			return transform.position + settings.followPoint;
		}

		public Vector3 GetRootPosition()
		{
			return transform.position;
		}

		public Quaternion GetModelRotation()
		{
			return transform.rotation;
		}

		private void OnChanged()
		{
			if (!settings.isCustom)
			{
				if (settings.spawnPlace == SpawnPlace.Forward)
				{
					settings.followPoint = new Vector3(0, 4, 2);
					settings.lookAtPoint = settings.followPoint;
					settings.tracketObjectOffset = new Vector3(0, 1, -1);

					transform.rotation = Quaternion.Euler(0, 0, 0);
				}
				else if (settings.spawnPlace == SpawnPlace.Backward)
				{
					settings.followPoint = new Vector3(0, 4, -2);
					settings.lookAtPoint = settings.followPoint;
					settings.tracketObjectOffset = new Vector3(0, 1, 1);

					transform.rotation = Quaternion.Euler(0, 180, 0);
				}
				else if (settings.spawnPlace == SpawnPlace.Left)
				{
					settings.followPoint = new Vector3(-2, 4, 0);
					settings.lookAtPoint = settings.followPoint;
					settings.tracketObjectOffset = new Vector3(1, 1, 0);

					transform.rotation = Quaternion.Euler(0, -90, 0);
				}
				else if (settings.spawnPlace == SpawnPlace.Right)
				{
					settings.followPoint = new Vector3(2, 4, 0);
					settings.lookAtPoint = settings.followPoint;
					settings.tracketObjectOffset = new Vector3(-1, 1, 0);

					transform.rotation = Quaternion.Euler(0, 90, 0);
				}

#if UNITY_EDITOR
				EditorUtility.SetDirty(this);
#endif
			}
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			DrawArrow.ForGizmo(transform.position, transform.forward);
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(transform.position, 0.15f);

			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position + settings.followPoint, 0.15f);

			Gizmos.color = Color.gray;
			Gizmos.DrawSphere(transform.position + settings.lookAtPoint, 0.1f);

			Gizmos.color = Color.blue;
			DrawArrow.ForGizmo(transform.position + settings.followPoint, settings.tracketObjectOffset);
		}
#endif

		[System.Serializable]
		public class Settings
		{
			public bool isCustom = false;
			[ShowIf("isCustom")]
			public Vector3 followPoint;
			[ShowIf("isCustom")]
			public Vector3 lookAtPoint;

			[ShowIf("isCustom")]
			public Vector3 tracketObjectOffset;
			
			[HideIf("isCustom")]
			public SpawnPlace spawnPlace = SpawnPlace.Forward;

			public SpawnType spawnType;
		}
	}

	public enum SpawnPlace
	{
		Left,
		Right,
		Forward,
		Backward,
	}

	public enum SpawnType
	{
		Player,
	}
}
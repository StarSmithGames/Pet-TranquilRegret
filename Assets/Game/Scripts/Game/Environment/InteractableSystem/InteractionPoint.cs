using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Environment.InteractionSystem
{
    public class InteractionPoint : MonoBehaviour
    {
		public Settings InteractionSettings => settings;
		[SerializeField] private Settings settings;

		public Vector3 GetIteractionPosition(Vector3 from)
		{
			if (settings.interaction == InteractionType.CustomPoint)
			{
				return transform.TransformPoint(settings.position);
			}
			else
			{
				if (IsInRange(from)) return from;

				return transform.position + ((settings.maxRange - 0.1f) * (from - transform.position).normalized);
			}
		}

		public Vector3 GetInteractionPosition()
		{
			if (settings.interaction == InteractionType.CustomPoint)
			{
				return transform.TransformPoint(settings.position);
			}

			return transform.position;
		}

		public bool IsInRange(Vector3 position)
		{
			float distance = Vector3.Distance(transform.position, position);

			return distance <= settings.maxRange;
		}


#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Gizmos.color = settings.gizmosEditor;
			if (settings.interaction == InteractionType.CustomPoint)
			{
				Handles.color = settings.gizmosEditor;
				Handles.DrawSolidDisc(transform.TransformPoint(settings.position), transform.up, 0.25f);
			}
			else
			{
				Gizmos.DrawWireSphere(transform.position, settings.maxRange);
			}
			var style = new GUIStyle();
			style.normal.textColor = settings.gizmosEditor;

			Handles.Label(transform.position + (Vector3.right * settings.maxRange), transform.name, style);
		}
#endif

		[System.Serializable]
		public class Settings
		{
			[Min(0.5f)]
			public float maxRange = 3f;

			public InteractionType interaction = InteractionType.DirectionPoint;
			[ShowIf("interaction", InteractionType.CustomPoint)]
			public Vector3 position;

			[ColorPalette("Gizmos")]
			public Color gizmosEditor;
		}
	}

	public enum InteractionType
	{
		DirectionPoint,
		CustomPoint,
	}

	public static class Range
	{
		public static bool IsIn(Vector3 origin, Vector3 position, float range)
		{
			return Vector3.Distance(origin, position) <= range;
		}
	}
}
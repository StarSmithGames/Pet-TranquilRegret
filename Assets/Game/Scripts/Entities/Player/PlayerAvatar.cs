using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Entities
{
	public class PlayerAvatar : MonoBehaviour
	{
		[field: SerializeField] public Transform CameraFollowPivot { get; private set; }
		[field: SerializeField] public Transform CameraLookAtPivot { get; private set; }
		[field: Space]
		[field: SerializeField] public Transform LeftHand { get; private set; }
		[field: SerializeField] public Transform LeftRightHand { get; private set; }
		[field: SerializeField] public Transform BothHandsPoint { get; private set; }

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (CameraLookAtPivot != null)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(CameraLookAtPivot.position, 0.2f);
			}

			if (CameraFollowPivot != null)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawSphere(CameraFollowPivot.position, 0.2f);
			}

			var style = new GUIStyle();
			style.normal.textColor = Color.green;

			Gizmos.color = Color.green;
			Gizmos.DrawSphere(BothHandsPoint.position, 0.05f);
			Handles.Label(BothHandsPoint.position + Vector3.down * 0.1f, "Hands", style);
			Gizmos.color = Color.blue;
			DrawArrow.ForGizmo(BothHandsPoint.position, BothHandsPoint.forward * 0.5f);
		}
#endif
	}
}
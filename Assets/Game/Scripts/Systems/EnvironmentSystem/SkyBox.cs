using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.EnvironmentSystem
{
#if UNITY_EDITOR
	[ExecuteAlways]
#endif
	public class SkyBox : MonoBehaviour
	{
		public Vector3 startRotation = Vector3.zero;
		public Vector3 rotationSpeed = Vector3.zero;

		private string rotationProperty = "_Rotation";
		private Vector3 currentRotation;

		private void Start()
		{
			currentRotation = startRotation;
			UpdateStartRotation();
		}

		private void Update()
		{
#if UNITY_EDITOR
			if (Application.isPlaying)
			{
#endif
				UpdateRotation();
#if UNITY_EDITOR
			}
			else
			{
				UpdateStartRotation();
			}
#endif
		}

		private void UpdateStartRotation()
		{
			var rot = Quaternion.Euler(startRotation);
			var matrix = Matrix4x4.TRS(Vector3.zero, rot, new Vector3(1, 1, 1));
			RenderSettings.skybox?.SetMatrix(rotationProperty, matrix);
		}

		private void UpdateRotation()
		{
			currentRotation += rotationSpeed * Time.deltaTime;
			var rot = Quaternion.Euler(currentRotation);
			var matrix = Matrix4x4.TRS(Vector3.zero, rot, new Vector3(1, 1, 1));
			RenderSettings.skybox?.SetMatrix(rotationProperty, matrix);
		}
	}
}
using Game.Extensions;
using Game.HUD.Menu;
using Game.Installers;
using Game.Managers.SwipeManager;
using Game.Systems.InfinityRoadSystem;

using Sirenix.OdinInspector;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;

using Zenject;

namespace Game.Systems.CameraSystem
{
	public class VerticalCamera : MonoBehaviour
	{
		public Vector3 TopPoint
		{
			get
			{
				if(roadMap == null)
				{
					roadMap = FindAnyObjectByType<MenuInstaller>().roadMap;
				}

				var bounds = roadMap.GetSprites().CalculateBounds();
				var point = Vector3.zero;
				point.y += bounds.size.y;
				point.y += BottomPoint.y;

				return point + topOffset;
			}
		}

		public Vector3 TopPointCamera
		{
			get
			{
				var point = TopPoint;
				point.y -= Camera.main.orthographicSize;

				return point;
			}
		}

		public Vector3 BottomPoint => startPoint;

		public Vector3 BottomPointCamera
		{
			get
			{
				var point = BottomPoint;
				point.y += Camera.main.orthographicSize;

				return point;
			}
		}

		public float mouseSensitivity = 1.0f;
		public float dragDamping = 0.1f;
		[Space]
		public Vector3 startPoint;
		public Vector3 topOffset;

		private bool isSwiping = false;
		private float swipeTime = 0.1f;
		private float t = 0;

		private Vector3 lastPosition;
		private Vector3 velocity;

		public CameraOrtographics Ortographics
		{
			get
			{
				if(ortographics == null)
				{
					ortographics = new(Camera.main);
				}
				return ortographics;
			}
		}
		private CameraOrtographics ortographics;

		[Inject] private RoadMap roadMap;
		[Inject] private SwipeManager swipeManager;

		private void Awake()
		{
			RefreshCamera();
			swipeManager.OnSwipeDetected += OnSwipeDetected;
		}

		private void Update()
		{
			if (!IsPointerOverUIObject())
			{
				if (Input.GetMouseButtonDown(0))
				{
					isSwiping = true;
					t = 0;
					lastPosition = Input.mousePosition;
				}
			}

			if (Input.GetMouseButtonUp(0))
			{
				isSwiping = false;
			}

			if (isSwiping)
			{
				PanCamera();

				t += Time.deltaTime;
			}

			ClampTransform();
		}

		public void SetPosition(Vector3 position)
		{
			position.x = 0;
			position.z = transform.position.z;
			transform.position = position;

			ClampTransform();
		}

		private void ClampTransform()
		{
			transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, BottomPointCamera.y, TopPointCamera.y), transform.position.z);
		}

		private void PanCamera()
		{
			if (Input.GetMouseButton(0))
			{
				Vector3 delta = lastPosition - Input.mousePosition;
				transform.Translate(0, delta.y * mouseSensitivity, 0);
				lastPosition = Input.mousePosition;
			}
		}

		//private Vector3 GetWorldPosition(float z)
		//{
		//	Ray mousePos = camera.ScreenPointToRay(Input.mousePosition);
		//	Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, z));
		//	float distance;
		//	ground.Raycast(mousePos, out distance);
		//	return mousePos.GetPoint(distance);
		//}

		//private Vector3 GetWorldMousePosition()
		//{
		//	return camera.ScreenToWorldPoint(GetMousePosition());
		//}

		//private Vector3 GetMousePosition()
		//{
		//	Vector3 mousePos = Input.mousePosition;
		//	mousePos.z = camera.nearClipPlane;

		//	return mousePos;
		//}

		//private void SwipeCamera()
		//{
		//	transform.position += velocity * Time.deltaTime;
		//	velocity *= Mathf.Pow(dragDamping, Time.deltaTime);
		//}

		private bool IsPointerOverUIObject()
		{
			if (EventSystem.current.IsPointerOverGameObject())//Windows
			{
				return true;
			}
			//Mobile
			PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
			eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
			return results.Count > 0;
		}

		private void RefreshCamera()
		{
			var sprites = roadMap.GetSprites();
			var bounds = sprites.CalculateBounds();
			var height = bounds.size.x / CameraUtilits.GetAspectTarget();
			var newOrtho = height / 2.0f;
			Ortographics.SetSize(newOrtho);
		}

		private void OnSwipeDetected(Swipe swipeDirection, Vector2 swipeVelocity)
		{
			velocity = swipeVelocity;
			velocity.x = 0;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;

			Gizmos.DrawSphere(startPoint, 1f);
			Gizmos.DrawLine(transform.position, BottomPoint);
			Gizmos.DrawLine(transform.position, TopPoint);
		}
	}
}
using Game.Managers.SwipeManager;
using Game.Systems.CameraSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Systems.InfinityRoadSystem
{
	public sealed partial class RoadCamera : MonoBehaviour
	{
		public Camera Camera;
		[Space]
		public float MouseSensitivity = 0.1f;
		public float DragDamping = 0.0f;
		[Space]
		public Vector3 TopOffset = Vector3.zero;

		private Vector3 _startPoint;
		private bool isSwiping = false;
		private float swipeTime = 0.1f;
		private float t = 0;

		private Vector3 lastPosition;
		private Vector3 velocity;

		private Bounds _roadBounds;
		
		public void SetBounds( Bounds roadBounds, Vector3 startPoint )
		{
			_roadBounds = roadBounds;
			_startPoint = startPoint;
			
			RefreshCamera();
			
			void RefreshCamera()
			{
				var height = _roadBounds.size.x / Camera.aspect;
				var newOrtho = height / 2.0f;
				Camera.orthographicSize = newOrtho;
			}
		}

		/// <summary>
		/// TODO: REmove
		/// </summary>
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
		
		public void Swipe( Swipe swipeDirection, Vector2 swipeVelocity)
		{
			velocity = swipeVelocity;
			velocity.x = 0;
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
				transform.Translate(0, delta.y * MouseSensitivity, 0);
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


	}

	public sealed partial class RoadCamera
	{
		public Vector3 TopPoint
		{
			get
			{
				var point = Vector3.zero;
				point.y += _roadBounds.size.y;
				point.y += BottomPoint.y;

				return point + TopOffset;
			}
		}

		public Vector3 TopPointCamera
		{
			get
			{
				var point = TopPoint;
				point.y -= Camera.orthographicSize;

				return point;
			}
		}

		public Vector3 BottomPoint => _startPoint;

		public Vector3 BottomPointCamera
		{
			get
			{
				var point = BottomPoint;
				point.y += Camera.orthographicSize;

				return point;
			}
		}
	}
}
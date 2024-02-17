using Game.Extensions;
using Game.Managers.SwipeManager;
using Game.Systems.InfinityRoadSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Systems.CameraSystem
{
	public sealed partial class VerticalCamera
	{
		private bool isSwiping = false;
		private float swipeTime = 0.1f;
		private float t = 0;

		private Vector3 lastPosition;
		private Vector3 velocity;

		private readonly VerticalCameraSettings _settings;
		private readonly Camera _camera;
		private readonly Transform _transform;
		private readonly RoadMap _roadMap;
		
		public VerticalCamera(
			VerticalCameraSettings settings,
			Camera camera,
			Transform transform,
			RoadMap roadMap
			)
		{
			_settings = settings ?? throw new ArgumentNullException( nameof(settings) );
			_camera = camera ?? throw new ArgumentNullException( nameof(camera) );
			_transform = transform ?? throw new ArgumentNullException( nameof(transform) );
			_roadMap = roadMap ?? throw new ArgumentNullException( nameof(roadMap) );
			
			RefreshCamera();
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
		
		public void Swipe( Swipe swipeDirection, Vector2 swipeVelocity)
		{
			velocity = swipeVelocity;
			velocity.x = 0;
		}

		public void SetPosition(Vector3 position)
		{
			position.x = 0;
			position.z = _transform.position.z;
			_transform.position = position;

			ClampTransform();
		}

		private void ClampTransform()
		{
			_transform.position = new Vector3(_transform.position.x, Mathf.Clamp(_transform.position.y, BottomPointCamera.y, TopPointCamera.y), _transform.position.z);
		}

		private void PanCamera()
		{
			if (Input.GetMouseButton(0))
			{
				Vector3 delta = lastPosition - Input.mousePosition;
				_transform.Translate(0, delta.y * _settings.MouseSensitivity, 0);
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
			var sprites = _roadMap.GetSprites();
			var bounds = sprites.CalculateBounds();
			var height = bounds.size.x / CameraUtilits.GetAspectTarget();
			var newOrtho = height / 2.0f;
			_camera.orthographicSize = newOrtho;
		}

		// private void OnDrawGizmos()
		// {
		// 	Gizmos.color = Color.red;
		//
		// 	Gizmos.DrawSphere(startPoint, 1f);
		// 	Gizmos.DrawLine(transform.position, BottomPoint);
		// 	Gizmos.DrawLine(transform.position, TopPoint);
		// }
	}

	public sealed partial class VerticalCamera
	{
		public Vector3 TopPoint
		{
			get
			{
				var bounds = _roadMap.GetSprites().CalculateBounds();
				var point = Vector3.zero;
				point.y += bounds.size.y;
				point.y += BottomPoint.y;

				return point + _settings.TopOffset;
			}
		}

		public Vector3 TopPointCamera
		{
			get
			{
				var point = TopPoint;
				point.y -= _camera.orthographicSize;

				return point;
			}
		}

		public Vector3 BottomPoint => _settings.StartPoint;

		public Vector3 BottomPointCamera
		{
			get
			{
				var point = BottomPoint;
				point.y += _camera.orthographicSize;

				return point;
			}
		}
	}
}
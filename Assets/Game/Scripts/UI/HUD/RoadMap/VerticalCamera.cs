using Game.HUD.Menu;
using Game.Managers.SwipeManager;

using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;

using Zenject;

public class VerticalCamera : MonoBehaviour
{
	public float FrustumHeight {
		get
		{
			if (frustumHeight == 0)
			{
				frustumHeight = 2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
			}

			return frustumHeight;
		}
	}
	private float frustumHeight = 0;
	public float FrustumWidth
	{
		get
		{
			if(frustumWidth == 0)
			{
				frustumWidth = FrustumHeight * camera.aspect;
			}

			return frustumWidth;
		}
	}
	private float frustumWidth = 0;

	public Vector3 TopPoint
	{
		get
		{
			if (topPoint == Vector3.zero)
			{
				topPoint = menu.sprites.Last().transform.position;
				topPoint.y += menu.sprites.Last().bounds.size.y / 2;
			}

			return topPoint + topOffset;
		}
	}
	private Vector3 topPoint;

	public Vector3 BottomPoint
	{
		get
		{
			if(bottomPoint == Vector3.zero)
			{
				bottomPoint = transform.position + new Vector3(0, -(FrustumHeight / 2), distance);
			}

			return bottomPoint + bottomOffset;
		}
	}
	private Vector3 bottomPoint;

	public Camera camera;
	public float distance = 100;
	public RoadMap menu;
	[Space]
	public float mouseSensitivity = 1.0f;
	public float dragDamping = 0.1f;
	[Space]
	public Vector3 topOffset;
	public Vector3 bottomOffset;

	private bool isSwiping = false;
	private float swipeTime = 0.1f;
	private float t = 0;

	private Vector3 lastPosition;
	private Vector3 velocity;

	private SwipeManager swipeManager;

	[Inject]
	private void Construct(SwipeManager swipeManager)
	{
		this.swipeManager = swipeManager;
	}

	private void Start()
	{
		swipeManager.OnSwipeDetected += OnSwipeDetected;
	}

	private void Update()
	{
		if(!IsPointerOverUIObject())
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

		ClampTransform();

		if (isSwiping)
		{
			PanCamera();

			t += Time.deltaTime;
		}
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
		var min = BottomPoint.y  + FrustumHeight / 2;
		var max = TopPoint.y - FrustumHeight / 2;
		transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, min, max), transform.position.z);
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

	private Vector3 GetWorldPosition(float z)
	{
		Ray mousePos = camera.ScreenPointToRay(Input.mousePosition);
		Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, z));
		float distance;
		ground.Raycast(mousePos, out distance);
		return mousePos.GetPoint(distance);
	}

	private Vector3 GetWorldMousePosition()
	{
		return camera.ScreenToWorldPoint(GetMousePosition());
	}

	private Vector3 GetMousePosition()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = camera.nearClipPlane;

		return mousePos;
	}

	private void SwipeCamera()
	{
		transform.position += velocity * Time.deltaTime;
		velocity *= Mathf.Pow(dragDamping, Time.deltaTime);
	}

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

	private void OnSwipeDetected(Swipe swipeDirection, Vector2 swipeVelocity)
	{
		velocity = swipeVelocity;
		velocity.x = 0;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawLine(transform.position, BottomPoint);
		Gizmos.DrawLine(transform.position, TopPoint);
	}
}
using Game.Entities;
using Game.UI;

using UnityEngine;
using UnityEngine.Events;

using Zenject;

namespace Game.Systems.NavigationSystem
{
	public class CharacterController : MonoBehaviour
	{
		public UnityAction onJumped;
		public UnityAction onLanded;

		public bool IsJumping { get; private set; }
		public bool IsFalling => rb.velocity.y < 0;
		public bool IsGrounded { get; private set; } = false;
		public GroundLayer CurrentGroundLayer { get; private set; }

		public float MovingMagnitude => directionVector.magnitude;

		[SerializeField] private Player player;
		[SerializeField] private Rigidbody rb;
		[SerializeField] private Transform model;
		[SerializeField] private Settings settings;

		private Vector3 directionVector;
		private Vector3 moveVector;
		private float turnSmoothTime = 0.1f;
		private float smoothVelocity;


		private UIGameCanvas subCanvas;
		private CameraSystem.CameraSystem cameraSystem;

		[Inject]
		private void Construct(UIGameCanvas subCanvas, CameraSystem.CameraSystem cameraSystem)
		{
			this.subCanvas = subCanvas;
			this.cameraSystem = cameraSystem;
		}

		private void Start()
		{
			CheckGround();
		}

		private void Update()
		{
			CheckGround();

			//Rotation
			directionVector = GetDirection();

			if (directionVector.x != 0 || directionVector.z != 0)
			{
				float targetAngle = Mathf.Atan2(directionVector.x, directionVector.z) * Mathf.Rad2Deg + cameraSystem.EulerAngleY;
				float angle = Mathf.SmoothDampAngle(model.eulerAngles.y, targetAngle, ref smoothVelocity, turnSmoothTime);

				model.rotation = Quaternion.Euler(0, angle, 0);
			}

			//Movement
			moveVector = Vector3.zero;

			if (IsGrounded)
			{
				moveVector = GetRelativeToCamera(GetDirection()) * player.Sheet.MoveSpeed.TotalValue * Time.deltaTime;

				if (Input.GetButtonDown("Jump"))
				{
					rb.AddForce(GetJumpImpulse(), ForceMode.Impulse);

					IsJumping = true;

					onJumped?.Invoke();
				}
			}
			else
			{
				moveVector = GetRelativeToCamera(GetDirection()) * player.Sheet.MoveSpeed.TotalValue * Time.deltaTime;
			}

			rb.MovePosition(rb.position + moveVector);
		}

		private void FixedUpdate()
		{
			if (rb.velocity.y < 0)
			{
				rb.velocity += Vector3.up * Physics.gravity.y * settings.fallMultipier * Time.fixedDeltaTime;
			}
		}

		public bool IsMoving()
		{
			return directionVector.x != 0 || directionVector.z != 0;
		}

		public bool IsIdling()
		{
			return directionVector.x == 0 && directionVector.z == 0;
		}

		private Vector3 GetDirection()
		{
			return new Vector3(subCanvas.Joystick.Horizontal, 0, subCanvas.Joystick.Vertical);
		}

		private Vector3 GetRelativeToCamera(Vector3 direction)
		{
			Vector3 forward = cameraSystem.Forward;
			Vector3 right = cameraSystem.Right;

			forward.y = 0f;
			right.y = 0f;

			return right.normalized * direction.x + forward.normalized * direction.z;
		}

		private void CheckGround()
		{
			if (Physics.Raycast(new Ray(transform.position, Vector3.down), out RaycastHit hit, settings.disstanceToGround, settings.groundLayer))
			{
				CurrentGroundLayer = hit.collider.GetComponent<GroundLayer>();

				IsGrounded = true;
				OnGroundedChanged();
			}
			else
			{
				CurrentGroundLayer = null;

				IsGrounded = false;
				OnGroundedChanged();
			}
		}

		private Vector3 GetJumpImpulse()
		{
			return new Vector3(0, player.Sheet.JumpImpulse.TotalValue, 0);
		}

		private void OnGroundedChanged()
		{
			if (IsJumping)
			{
				if (IsGrounded && IsFalling)
				{
					onLanded?.Invoke();

					IsJumping = false;
				}
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.DrawLine(transform.position, transform.position + Vector3.down * (settings.disstanceToGround));
		}

		[System.Serializable]
		public class Settings
		{
			public float fallMultipier = 1f;
			[Space]
			public LayerMask groundLayer;
			public float disstanceToGround = 0.5f;
		}
	}
}
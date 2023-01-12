using Game.Entities;

using UnityEngine;
using UnityEngine.Events;

using Zenject;

namespace Game.Systems.NavigationSystem
{
	public class CharacterController : MonoBehaviour
	{
		public UnityAction onLanded;

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

		private bool isJumping;

		private Joystick joystick;
		private CameraSystem.CameraSystem cameraSystem;

		[Inject]
		private void Construct(Joystick joystick, CameraSystem.CameraSystem cameraSystem)
		{
			this.joystick = joystick;
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
				moveVector = GetRelativeToCamera(GetDirection()) * settings.moveSpeed * Time.deltaTime;

				if (Input.GetButtonDown("Jump"))
				{
					rb.AddForce(GetJumpImpulse(), ForceMode.Impulse);

					isJumping = true;
				}
			}
			else
			{
				moveVector = GetRelativeToCamera(GetDirection()) * settings.moveSpeed * Time.deltaTime;
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
			return new Vector3(joystick.Horizontal, 0, joystick.Vertical);
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
			var jumpImpulse = player.Effects.GetJumpImpulse();

			if(jumpImpulse != Vector3.zero)
			{
				return jumpImpulse;
			}

			return settings.jumpImpulse;
		}

		private void OnGroundedChanged()
		{
			if (isJumping)
			{
				if (IsGrounded && IsFalling)
				{
					onLanded?.Invoke();

					isJumping = false;
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
			public float moveSpeed = 7.5f;
			public Vector3 jumpImpulse = new Vector3(0, 5, 0);
			public float fallMultipier = 1f;
			[Space]
			public LayerMask groundLayer;
			public float disstanceToGround = 0.5f;
		}
	}
}
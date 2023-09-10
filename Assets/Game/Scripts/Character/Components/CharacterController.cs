using Game.Entities;
using Game.Systems.CameraSystem;
using Game.Systems.NavigationSystem;
using Game.UI;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

using Zenject;

namespace Game.Character
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
		
		[Inject] private AbstractCharacter character;
		[Inject] private Rigidbody rb;
		[Inject] private CharacterConfig config;

		private Vector3 directionVector;
		private Vector3 moveVector;
		private float turnSmoothTime = 0.1f;
		private float smoothVelocity;


		private UIGameCanvas subCanvas;
		private CameraSystem cameraSystem;

		[Inject]
		private void Construct(UICanvas subCanvas, CameraSystem cameraSystem)
		{
			this.subCanvas = subCanvas as UIGameCanvas;
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
				float angle = Mathf.SmoothDampAngle(character.model.eulerAngles.y, targetAngle, ref smoothVelocity, turnSmoothTime);

				character.model.rotation = Quaternion.Euler(0, angle, 0);
			}

			//Movement
			moveVector = Vector3.zero;

			if (IsGrounded)
			{
				moveVector = GetRelativeToCamera(GetDirection()) * character.sheet.MoveSpeed.TotalValue * Time.deltaTime;

				if (Input.GetButtonDown("Jump"))
				{
					rb.AddForce(GetJumpImpulse(), ForceMode.Impulse);

					IsJumping = true;

					onJumped?.Invoke();
				}
			}
			else
			{
				moveVector = GetRelativeToCamera(GetDirection()) * character.sheet.MoveSpeed.TotalValue * Time.deltaTime;
			}

			rb.MovePosition(rb.position + moveVector);
		}

		private void FixedUpdate()
		{
			if (rb.velocity.y < 0)
			{
				rb.velocity += Vector3.up * Physics.gravity.y * config.controllSettings.fallMultipier * Time.fixedDeltaTime;
			}
		}

		public bool IsMoving()
		{
			return Mathf.Abs(directionVector.x) > config.controllSettings.thresholdIdle || Mathf.Abs(directionVector.z) > config.controllSettings.thresholdIdle;
		}

		public bool IsIdling()
		{
			return Mathf.Abs(directionVector.x) < config.controllSettings.thresholdIdle && Mathf.Abs(directionVector.z) < config.controllSettings.thresholdIdle;
		}

		private Vector3 GetDirection()
		{
			Vector3 direction = new Vector3(subCanvas.Joystick.Horizontal, 0, subCanvas.Joystick.Vertical);

			if(direction == Vector3.zero)
			{
				direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			}

			return direction;
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
			if (Physics.Raycast(new Ray(transform.position, Vector3.down), out RaycastHit hit, config.controllSettings.disstanceToGround, config.controllSettings.groundLayer))
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
			return new Vector3(0, character.sheet.JumpImpulse.TotalValue, 0);
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

		private void OnDrawGizmosSelected()
		{
			var c = GetComponentInParent<CharacterInstaller>().config;
			Assert.IsNotNull(c);
			Gizmos.DrawLine(transform.position, transform.position + Vector3.down * (c.controllSettings.disstanceToGround));
		}

		[System.Serializable]
		public class Settings
		{
			public float thresholdIdle = 0.05f;

			[Header("Gravity")]
			public float fallMultipier = 1f;
			[Space]
			public LayerMask groundLayer;
			public float disstanceToGround = 0.5f;
		}
	}
}
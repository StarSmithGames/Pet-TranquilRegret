using Game.Entities;
using Game.Managers.PauseManager;
using Game.Services;
using Game.Systems.CameraSystem;
using Game.Systems.NavigationSystem;
using Game.Systems.UISystem;
using Game.UI;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

using Zenject;

namespace Game.Entity.CharacterSystem
{
	public class CharacterController : MonoBehaviour, IPausable
	{
		public event UnityAction onJumped;
		public event UnityAction onLanded;

		public bool IsJumping { get; private set; }
		public bool IsFalling => rb.velocity.y < 0;
		public bool IsGrounded { get; private set; } = false;
		public GroundLayer CurrentGroundLayer { get; private set; }

		public float MovingMagnitude => directionVector.magnitude;
		private Settings ControllSettings => presenter.Model.Config.controllSettings;

		private bool isPaused;
		private UIJoystick jostick;
		private Vector3 directionVector;
		private Vector3 moveVector;
		private float turnSmoothTime = 0.1f;
		private float smoothVelocity;

		[Inject] private CharacterPresenter presenter;
		[Inject] private Rigidbody rb;

		[Inject] private UIRootGame uiRoot;
		[Inject] private CameraSystem cameraSystem;
		[Inject] private PauseManager pauseManager;

		private void Start()
		{
			pauseManager.Registrate(this);

			jostick = uiRoot.ControlCanvas.Joystick;
			CheckGround();
		}

		private void OnDestroy()
		{
			pauseManager.UnRegistrate(this);
		}

		private void Update()
		{
			CheckGround();

			if (isPaused) return;

			//Rotation
			directionVector = GetDirection();

			if (directionVector.x != 0 || directionVector.z != 0)
			{
				float targetAngle = Mathf.Atan2(directionVector.x, directionVector.z) * Mathf.Rad2Deg + cameraSystem.EulerAngleY;
				float angle = Mathf.SmoothDampAngle(presenter.View.model.eulerAngles.y, targetAngle, ref smoothVelocity, turnSmoothTime);

				presenter.View.model.rotation = Quaternion.Euler(0, angle, 0);
			}

			//Movement
			moveVector = GetRelativeToCamera(GetDirection()) * presenter.Model.Sheet.MoveSpeed.TotalValue * Time.deltaTime;

			if (IsGrounded)
			{
				if (Input.GetButtonDown("Jump"))
				{
					Jump();
				}
			}

			rb.MovePosition(rb.position + moveVector);
		}

		public void Pause()
		{
			isPaused = true;
		}

		public void UnPause()
		{
			isPaused = false;
		}

		public void Jump()
		{
			if (IsGrounded)
			{
				rb.AddForce(GetJumpImpulse(), ForceMode.Impulse);

				IsJumping = true;

				onJumped?.Invoke();
			}
		}

		public bool IsMoving()
		{
			return Mathf.Abs(directionVector.x) > ControllSettings.thresholdIdle || Mathf.Abs(directionVector.z) > ControllSettings.thresholdIdle;
		}

		public bool IsIdling()
		{
			return Mathf.Abs(directionVector.x) < ControllSettings.thresholdIdle && Mathf.Abs(directionVector.z) < ControllSettings.thresholdIdle;
		}

		private Vector3 GetDirection()
		{
			Vector3 direction = new Vector3(jostick.Horizontal, 0, jostick.Vertical);

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
			if (Physics.Raycast(new Ray(transform.position, Vector3.down), out RaycastHit hit, ControllSettings.disstanceToGround, ControllSettings.groundLayer))
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
			return new Vector3(0, presenter.Model.Sheet.JumpImpulse.TotalValue, 0);
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
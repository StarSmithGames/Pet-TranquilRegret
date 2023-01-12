using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

using Zenject;

namespace Game.Systems.NavigationSystem
{
	public class CharacterController : MonoBehaviour
	{
		[SerializeField] private Player player;
		[SerializeField] private Rigidbody rb;
		[SerializeField] private Transform model;
		[SerializeField] private Settings settings;

		private float turnSmoothTime = 0.1f;
		private float smoothVelocity;

		private Joystick joystick;
		private CameraSystem.CameraSystem cameraSystem;

		[Inject]
		private void Construct(Joystick joystick, CameraSystem.CameraSystem cameraSystem)
		{
			this.joystick = joystick;
			this.cameraSystem = cameraSystem;
		}

		private void Update()
		{
			//Rotation
			Vector3 directionVector = GetDirection();

			if (directionVector.x != 0 || directionVector.z != 0)
			{
				float targetAngle = Mathf.Atan2(directionVector.x, directionVector.z) * Mathf.Rad2Deg + cameraSystem.EulerAngleY;
				float angle = Mathf.SmoothDampAngle(model.eulerAngles.y, targetAngle, ref smoothVelocity, turnSmoothTime);

				model.rotation = Quaternion.Euler(0, angle, 0);
			}

			//Movement
			Vector3 moveVector = Vector3.zero;

			if (IsGrounded())
			{
				moveVector = GetRelativeToCamera(GetDirection()) * settings.moveSpeed * Time.deltaTime;

				if (Input.GetButtonDown("Jump"))
				{
					rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
				}

				if (directionVector.x != 0 || directionVector.z != 0)
				{
					player.PlayerVFX.EnableDust(true);
					//_animatorController.PlayRun();
				}
				else if (directionVector.x == 0 && directionVector.z == 0)
				{
					player.PlayerVFX.EnableDust(false);
					//_animatorController.PlayIdle();
				}
			}
			else
			{
				moveVector = GetRelativeToCamera(GetDirection()) * settings.moveSpeed * Time.deltaTime;

				player.PlayerVFX.EnableDust(false);
			}

			rb.MovePosition(rb.position + moveVector);
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

		private bool IsGrounded()
		{
			return Physics.Raycast(transform.position, Vector3.down, settings.disstanceToGround, settings.groundLayer);
		}

		private void OnDrawGizmos()
		{
			Gizmos.DrawLine(transform.position, transform.position + Vector3.down * (settings.disstanceToGround));
		}

		[System.Serializable]
		public class Settings
		{
			public float moveSpeed = 7.5f;

			public LayerMask groundLayer;
			public float disstanceToGround = 0.5f;
		}
	}
}
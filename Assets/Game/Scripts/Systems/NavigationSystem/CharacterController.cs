using UnityEngine;

using Zenject;

namespace Game.Systems.NavigationSystem
{
	public class CharacterController : MonoBehaviour
	{
		public float moveSpeed = 7.5f;
		public float rotateSpeed = 10;

		[SerializeField] private Player player;
		[SerializeField] private Rigidbody rigidbody;
		[SerializeField] private Transform model;

		private Vector3 moveVector;

		private Joystick joystick;

		[Inject]
		private void Construct(Joystick joystick)
		{
			this.joystick = joystick;
		}

		private void FixedUpdate()
		{
			moveVector = Vector3.zero;
			moveVector.x = joystick.Horizontal * moveSpeed * Time.deltaTime;
			moveVector.z = joystick.Vertical * moveSpeed * Time.deltaTime;

			if (joystick.Horizontal != 0 || joystick.Vertical != 0)
			{
				Vector3 direction = Vector3.RotateTowards(model.forward, moveVector, rotateSpeed * Time.deltaTime, 0.0f);
				model.rotation = Quaternion.LookRotation(direction);

				player.PlayerVFX.EnableDust(true);
				//_animatorController.PlayRun();
			}
			else if (joystick.Horizontal == 0 && joystick.Vertical == 0)
			{
				player.PlayerVFX.EnableDust(false);
				//_animatorController.PlayIdle();
			}

			rigidbody.MovePosition(rigidbody.position + moveVector);



			//rigidbody.velocity = new Vector3(joystick.Horizontal * moveSpeed, rigidbody.velocity.y, joystick.Vertical * moveSpeed);

			//if (joystick.Horizontal != 0 || joystick.Vertical != 0)
			//{
			//	Vector3 direction = joystick.Direction;//.normalized;

			//	float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;// + character.CameraVision.transform.eulerAngles.y;
			//	float angle = Mathf.SmoothDampAngle(model.eulerAngles.y, targetAngle, ref smoothVelocity, turnSmoothTime);

			//	model.rotation = Quaternion.Euler(0, angle, 0);
			//}
		}
	}
}
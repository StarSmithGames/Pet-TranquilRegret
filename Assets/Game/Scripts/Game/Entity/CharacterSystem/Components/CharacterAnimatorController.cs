using UnityEngine;

using Zenject;

namespace Game.Entity.CharacterSystem
{
	/// TODO: Remove MonoBehaviour
	public class CharacterAnimatorController : MonoBehaviour
    {
        [Inject] private Animator animator;
		[Inject] private CharacterController characterController;

		protected int isIdleHash;
		protected int forwardSpeedHash;
		protected int horizontalTurnSpeedHash;
		protected int jumpHash;

		private void Awake()
		{
			isIdleHash = Animator.StringToHash("IsIdle");
			forwardSpeedHash = Animator.StringToHash("Forward");
			horizontalTurnSpeedHash = Animator.StringToHash("HorizontalTurn");
			jumpHash = Animator.StringToHash("Jump");

			characterController.onJumped += OnJumped;
		}

		private void OnDestroy()
		{
			if(characterController != null)
			{
				characterController.onJumped -= OnJumped;
			}
		}

		protected virtual void Update()
		{
			animator.SetBool(isIdleHash, characterController.IsIdling());
			animator.SetFloat(forwardSpeedHash, characterController.IsMoving() ? 1 : 0);
			animator.SetFloat(horizontalTurnSpeedHash, 0);
		}

		private void OnJumped()
		{
			animator.SetTrigger( jumpHash );
		}
	}
}
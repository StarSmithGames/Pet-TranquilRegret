using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Character
{
    public class CharacterAnimatorController : MonoBehaviour
    {
        [Inject] private Animator animator;
		[Inject] private CharacterController characterController;

		protected int isIdleHash;
		protected int forwardSpeedHash;
		protected int horizontalTurnSpeedHash;

		private void Awake()
		{
			isIdleHash = Animator.StringToHash("IsIdle");
			forwardSpeedHash = Animator.StringToHash("Forward");
			horizontalTurnSpeedHash = Animator.StringToHash("HorizontalTurn");
		}

		protected virtual void Update()
		{
			animator.SetBool(isIdleHash, characterController.IsIdling());
			animator.SetFloat(forwardSpeedHash, characterController.IsMoving() ? 1 : 0);
			animator.SetFloat(horizontalTurnSpeedHash, 0);
		}
	}
}
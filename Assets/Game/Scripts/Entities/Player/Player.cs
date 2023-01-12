using Game.Systems.InteractionSystem;
using Game.Systems.NavigationSystem;

using UnityEngine;

using Zenject;

namespace Game.Entities
{
	public interface ICharacter
	{
		Systems.NavigationSystem.CharacterController Controller { get; }

		Effects Effects { get; }
	}

	public class Player : MonoBehaviour, ICharacter
	{
		public Effects Effects { get; private set; }


		public Transform Model => model;
		[SerializeField] private Transform model;

		public Transform CameraFollowPivot => cameraFollowPivot;
		[SerializeField] private Transform cameraFollowPivot;

		public Transform CameraLookAtPivot => cameraLookAtPivot;
		[SerializeField] private Transform cameraLookAtPivot;

		public PlayerVFX PlayerVFX => playerVFX;
		[SerializeField] private PlayerVFX playerVFX;

		public Systems.NavigationSystem.CharacterController Controller => controller;
		[SerializeField] private Systems.NavigationSystem.CharacterController controller;

		public PlayerCanvas PlayerCanvas => playerCanvas;

		[SerializeField] private PlayerCanvas playerCanvas;

		private GroundImplementation groundImplementation;

		[Inject]
		private void Construct(GroudImplementationFactory groudImplementationFactory)
		{
			groundImplementation = groudImplementationFactory.Create(this);

			Effects = new Effects(this);
		}

		private void OnDrawGizmos()
		{
			if (cameraLookAtPivot != null)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(cameraLookAtPivot.position, 0.2f);
			}

			if (cameraFollowPivot != null)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawSphere(cameraFollowPivot.position, 0.2f);
			}
		}

		public class Factory : PlaceholderFactory<Player> { }
	}
}
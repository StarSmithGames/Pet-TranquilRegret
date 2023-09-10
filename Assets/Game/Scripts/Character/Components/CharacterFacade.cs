using UnityEngine;

using Zenject;

namespace Game.Character
{
	public class CharacterFacade : MonoBehaviour
	{
		public Transform cameraFollowPivot;
		public Transform cameraLookAtPivot;

		[Inject, HideInInspector] public CharacterCanvas characterCanvas;
		[Inject] public CharacterGroundImplementation groundImplementation;
	}
}
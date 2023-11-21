using UnityEngine;

namespace Game.Character
{
	public class CharacterView : MonoBehaviour
	{
		public Transform root;
		public Transform model;

		public Transform cameraFollowPivot;
		public Transform cameraLookAtPivot;
	}
}
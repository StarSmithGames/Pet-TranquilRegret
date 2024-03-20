using UnityEngine;

namespace Game.Entity.CharacterSystem
{
	[ System.Serializable ]
	public sealed class CharacterPoints
	{
		public Transform Root;
		public Transform Rotor;
		[ Space ]
		public Transform CameraFollowPivot;
		public Transform CameraLookAtPivot;
		[ Space ]
		public Transform LeftHandPoint;
		public Transform RightHandPoint;
		public Transform HeavyObjectsPoint;
	}
}
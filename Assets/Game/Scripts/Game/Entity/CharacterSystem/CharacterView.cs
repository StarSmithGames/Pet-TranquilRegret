using Game.Systems.CollisionSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Entity.CharacterSystem
{
	public class CharacterView : MonoBehaviour
	{
		public Transform root;
		public Transform model;

		public Transform cameraFollowPivot;
		public Transform cameraLookAtPivot;
		[ Space ]
		public DamagableEntityColliderCaster FrontSphereCaster;
		public List< ColliderCaster > Casters = new();

		#if UNITY_EDITOR
		[Button( DirtyOnClick = true )]
		private void FillAll()
		{
			Casters = GetComponentsInChildren< ColliderCaster >().ToList();
		}
		#endif
	}
}
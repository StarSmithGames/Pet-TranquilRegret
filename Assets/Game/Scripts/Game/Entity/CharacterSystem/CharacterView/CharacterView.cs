using Game.Systems.CollisionSystem;
using UnityEngine;

namespace Game.Entity.CharacterSystem
{
	public class CharacterView : MonoBehaviour
	{
		public CharacterPoints Points;
		public CharacterOutfit Outfit;
		public DamagableEntityColliderCaster FrontSphereCaster;
	}
}
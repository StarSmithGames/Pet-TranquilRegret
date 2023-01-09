using Game.Systems.FloatingSystem;
using UnityEngine;

namespace Game.Entities
{
    public class Coin : Floating3DObject
	{
		public int Count => count;

		[Min(1)]
		[SerializeField] private int count = 1;

		protected override void OnAnimationPathStart() { }
	}
}
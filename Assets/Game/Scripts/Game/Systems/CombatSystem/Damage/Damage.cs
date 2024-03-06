using Sirenix.OdinInspector;
using StarSmithGames.Core;
using UnityEngine;

namespace Game.Systems.CombatSystem
{
	public sealed class Damage
	{
		[ MinMaxSlider(0, 99, true) ]
		public Vector2 Amount;
		public DamageType DamageType;
		public Vector3 AttackDirection;
		
		public float DMG => Amount.RandomBtw();
	}
}
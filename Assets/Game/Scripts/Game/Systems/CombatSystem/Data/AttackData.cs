using UnityEngine;

namespace Game.Systems.CombatSystem.Data
{
	[ CreateAssetMenu( fileName = "AttackData", menuName = "Game/Combo/Attack") ]
	public sealed class AttackData : ScriptableObject
	{
		public float ImpactStartTime;
		public float ImpactEndTime;
	}
}
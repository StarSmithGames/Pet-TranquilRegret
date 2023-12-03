using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.PhysicsSystem
{
	[CreateAssetMenu(fileName = "PhysicsSettings", menuName = "Settings/PhysicsSettings")]
	public class PhysicsSettings : ScriptableObject
	{
		public PhysicMaterial frictionMin;
		public PhysicMaterial frictionMax;
		public PhysicMaterial bouncinessSmall;
	}
}
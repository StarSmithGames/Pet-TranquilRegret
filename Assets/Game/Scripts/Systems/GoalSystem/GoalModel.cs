using Game.Systems.FloatingSystem;

using UnityEngine;

namespace Game.Systems.GoalSystem
{
	public abstract class GoalModel : Floating3DObject
	{
		public Goal Goal { get; private set; }

		[Space]
		public GoalItem goal;
	}
}
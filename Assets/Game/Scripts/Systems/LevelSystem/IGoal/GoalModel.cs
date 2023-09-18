using Game.Systems.FloatingSystem;

using UnityEngine;

namespace Game.Systems.LevelSystem
{
	public abstract class GoalModel : Floating3DObject
	{
		public Goal Goal { get; private set; }

		[Space]
		public GoalItem goal;
	}
}
using EPOOutline;
using Game.Systems.GoalSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace Game.Systems.LevelSystem
{
	public sealed class LevelRegularView : LevelView
	{
		public List< GoalView > Goals = new();

		public List< Outlinable > Outlinables = new();

		public override IEnumerable< Outlinable > GetOutlinables() => Outlinables;
		
		[Button( DirtyOnClick = true )]
		private void Refresh()
		{
			Goals = GetComponentsInChildren< GoalView >().ToList();
			Outlinables = Goals.Select( ( x ) => x.Outlinable ).ToList();
		}
	}
}
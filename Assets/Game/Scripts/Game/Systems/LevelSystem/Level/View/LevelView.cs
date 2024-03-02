using EPOOutline;
using Game.VVM;
using System.Collections.Generic;

namespace Game.Systems.LevelSystem
{
	public abstract class LevelView : View
	{
		public abstract IEnumerable< Outlinable > GetOutlinables();
	}
}
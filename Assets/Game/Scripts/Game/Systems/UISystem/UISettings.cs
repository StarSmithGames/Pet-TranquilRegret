using StarSmithGames.Go;
using System.Collections.Generic;

namespace Game.UI
{
	[ System.Serializable ]
	public sealed class UISettings
	{
		public List< ViewBase > Dialogs = new();
	}
}
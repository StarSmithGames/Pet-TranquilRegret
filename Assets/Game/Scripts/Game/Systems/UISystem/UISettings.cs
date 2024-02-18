using StarSmithGames.Go;
using System.Collections.Generic;

namespace Game.Systems.UISystem
{
	[ System.Serializable ]
	public sealed class UISettings
	{
		public List< ViewBase > PreloadDialogs = new();
		public List< ViewBase > Dialogs = new();
	}
}
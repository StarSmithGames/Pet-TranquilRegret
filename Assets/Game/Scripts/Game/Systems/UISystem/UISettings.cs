using StarSmithGames.Go;
using System.Collections.Generic;

namespace Game.Systems.UISystem
{
	[ System.Serializable ]
	public sealed class UISettings
	{
		public List< ViewBase > PreloadDialogs = new();
		public List< ViewBase > MenuDialogs = new();
		public List< ViewBase > GameDialogs = new();
		public List< ViewBase > CommonDialogs = new();
	}
}
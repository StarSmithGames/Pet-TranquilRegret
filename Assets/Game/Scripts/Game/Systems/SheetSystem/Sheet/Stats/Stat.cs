using StarSmithGames.Core;

namespace Game.Systems.SheetSystem
{
	public abstract class Stat : Attribute
	{
		protected Stat( float value ) : base( value )
		{
		}
	}
}
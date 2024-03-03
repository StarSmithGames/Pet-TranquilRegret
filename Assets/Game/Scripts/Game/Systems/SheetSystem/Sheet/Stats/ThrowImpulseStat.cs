using StarSmithGames.Core;

namespace Game.Systems.SheetSystem
{
	public class ThrowImpulseStat : Stat, IEnableable
	{
		public bool IsEnable { get; private set; }

		public override float TotalValue => IsEnable ? base.TotalValue : 0;

		public ThrowImpulseStat( float value ) : base( value )
		{
		}

		public void Enable( bool trigger )
		{
			IsEnable = trigger;
		}
	}
}
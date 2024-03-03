using Game.Systems.StorageSystem;

namespace Game.Systems.BoosterManager
{
	public abstract class Booster
	{
		public BoosterData Data
		{
			get
			{
				if ( _data == null )
				{
					_data = GetData();
				}

				return _data;
			}
		}
		private BoosterData _data;
		
		protected abstract BoosterData GetData();
	}
}
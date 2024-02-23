using Zenject;

namespace Game.Managers.DIManager
{
	public sealed class DIManager
	{
		public DiContainer CurrentContainer { get; private set; }

		public void SetContainer( DiContainer container )
		{
			CurrentContainer = container;
		}
	}
}
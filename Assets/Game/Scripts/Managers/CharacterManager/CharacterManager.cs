using Game.Entities;
using Zenject;

namespace Game.Managers.CharacterManager
{
	public class CharacterManager
	{
		public Player CurrentPlayer { get; private set; }

		private SignalBus signalBus;

		public CharacterManager(SignalBus signalBus)
		{
			this.signalBus = signalBus;
		}

		public void SetPlayer(Player player)
		{
			CurrentPlayer = player;

			signalBus?.Fire(new SignalPlayerChanged() { player = player});
		}
	}
}
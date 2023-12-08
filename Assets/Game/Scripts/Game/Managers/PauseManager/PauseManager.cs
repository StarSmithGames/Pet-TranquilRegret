using StarSmithGames.Core;

using System;

namespace Game.Managers.PauseManager
{
	public sealed class PauseManager : Registrator<IPausable>
	{
		public event Action onPauseChanged;

		public bool IsPaused { get; private set; }

		public void Pause()
		{
			IsPaused = true;

			for (int i = 0; i < registers.Count; i++)
			{
				registers[i].Pause();
			}

			onPauseChanged?.Invoke();
		}

		public void UnPause()
		{
			IsPaused = false;

			for (int i = 0; i < registers.Count; i++)
			{
				registers[i].UnPause();
			}

			onPauseChanged?.Invoke();
		}
	}
}
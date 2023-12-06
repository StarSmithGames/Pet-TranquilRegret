using System;

using UnityEngine;

using Zenject;

namespace Game.Managers.GameManager
{
	public sealed class GameManager
	{
		public event Action onGameStateChanged;

		public bool IsMenu => CurrentGameState == GameState.Menu;
		public bool IsGame => CurrentGameState == GameState.PreGameplay || CurrentGameState == GameState.Gameplay;

		public GameState CurrentGameState { get; private set; } = GameState.Empty;
		public GameState PreviousGameState { get; private set; } = GameState.Empty;

		private SignalBus signalBus;

		public GameManager(SignalBus signalBus)
		{
			this.signalBus = signalBus;
		}

		public void ChangeState(GameState gameState)
		{
			if (CurrentGameState != gameState)
			{
				PreviousGameState = CurrentGameState;
				CurrentGameState = gameState;

				onGameStateChanged?.Invoke();

				signalBus?.Fire(new SignalGameStateChanged
				{
					newGameState = CurrentGameState,
					oldGameState = PreviousGameState
				});
			}
			else
			{
				Debug.LogError($"Try to set state: {gameState}, but it's already setted.");
			}
		}
	}
	public enum GameState
	{
		Empty,
		Loading,
		Menu,
		PreGameplay,
		Gameplay,
	}
}
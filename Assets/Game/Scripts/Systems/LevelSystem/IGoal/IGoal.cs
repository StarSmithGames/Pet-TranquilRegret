using StarSmithGames.Core;

using System;

using UnityEngine;

namespace Game.Systems.LevelSystem
{
	public interface IGoal : IValue<float>, IReadOnlyValue<float>, IObservableValue, IBar
	{
		GoalConfig Config { get; }
		bool IsCompleted { get; }
	}

	public class Goal : IGoal
	{
		public event Action onChanged;

		public GoalConfig Config { get; protected set; }

		public bool IsCompleted => CurrentValue == MaxValue;

		public float CurrentValue
		{
			get => currentValue;
			set
			{
				currentValue = Mathf.Clamp(value, MinValue, MaxValue);
				onChanged?.Invoke();
			}
		}
		protected float currentValue;

		public float MinValue { get; protected set; }
		public float MaxValue { get; protected set; }
		public float PercentValue => CurrentValue / MaxValue;

		public Goal(GoalConfig config, float max)
		{
			Config = config;

			MinValue = 0;
			MaxValue = max;

			CurrentValue = 0;
		}

		public Goal(GoalItem item)
		{
			Config = item.config;

			MinValue = 0;
			MaxValue = item.count;

			CurrentValue = 0;
		}
	}

	public class Coins : StarSmithGames.Core.Attribute
	{
		public Coins(int currentValue) : base(currentValue)
		{
		}
	}
}
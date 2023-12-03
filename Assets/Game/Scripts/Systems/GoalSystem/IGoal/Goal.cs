using System;

using UnityEngine;

namespace Game.Systems.GoalSystem
{
	public class Goal : IGoal
	{
		public event Action onChanged;

		public GoalConfigWrapper ConfigWrapper { get; protected set; }

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

		public Goal(GoalConfigWrapper wrapper)
		{
			ConfigWrapper = wrapper;

			MinValue = 0;
			MaxValue = 1;

			CurrentValue = 0;
		}
	}
}
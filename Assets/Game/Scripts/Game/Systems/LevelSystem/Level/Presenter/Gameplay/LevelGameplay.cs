using Game.Systems.GoalSystem;
using System;

namespace Game.Systems.LevelSystem
{
	public class LevelGameplay : IDisposable
	{
		public event Action OnCompleted;
		
		public GoalRegistrator GoalRegistrator { get; }

		public LevelGameplay( LevelConfig levelConfig)
		{
			GoalRegistrator = new( levelConfig );
			
			GoalRegistrator.onAccumulatedPrimary += OnGoalsChanged;
		}

		public void Dispose()
		{
			GoalRegistrator.onAccumulatedPrimary -= OnGoalsChanged;
		}
		
		public bool IsCompleted()
		{
			return GoalRegistrator.IsPrimaryGoalsCompleted();
		}

		private void OnGoalsChanged()
		{
			if ( IsCompleted() )
			{
				OnCompleted?.Invoke();
			}
		}
	}
}
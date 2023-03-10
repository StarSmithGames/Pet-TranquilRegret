namespace Game.Managers.LevelManager
{
	public interface IGoal
	{
		bool IsCompleted { get; }
	}

	public class GoalBar : AttributeBar, IGoal
	{
		public bool IsCompleted => CurrentValue == MaxValue;

		public override string LocalizationKey => Data.LocalizationTitleKey;

		public CountableGoalData Data { get; private set; }

		public GoalBar(CountableGoalData data, float value, float min, float max) : base(value, min, max)
		{
			this.Data = data;
		}
	}

	public class Coins : Attribute<int>
	{
		public Coins(int currentValue) : base(currentValue)
		{
		}
	}
}
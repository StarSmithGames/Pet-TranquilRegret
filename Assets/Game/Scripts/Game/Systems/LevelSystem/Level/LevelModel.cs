using Game.Systems.GoalSystem;

namespace Game.Systems.LevelSystem
{
	public class LevelModel
	{
		public LevelConfig Config { get; private set; }
		public GoalRegistrator GoalRegistrator { get; private set; }

		public LevelModel(LevelConfig config)
		{
			Config = config;
			GoalRegistrator = new GoalRegistrator(config);
		}

		public bool IsCompleted()
		{
			return GoalRegistrator.IsPrimaryGoalsCompleted();
		}
	}
}
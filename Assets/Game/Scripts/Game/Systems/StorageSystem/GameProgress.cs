using Game.Systems.GameSystem;

using System.Collections.Generic;

namespace Game.Systems.StorageSystem
{
	public sealed class GameProgress
	{
		public List<RegularLevelData> regularLevels = new();

		public GameProgress() { }
		
		public GameProgress(int count)
		{
			regularLevels = new(count);
			AddRegularLevels(count);
		}

		public void AddRegularLevels(int count)
		{
			for (int i = 0; i < count; i++)
			{
				regularLevels.Add(new());
			}
		}

		public int GetCurrentRegularIndex()
		{
			return regularLevels.FindIndex((data) => data.completed == 0);
		}
	}

	public sealed class RegularLevelData
	{
		public int stars = 0;
		public int completed = 0;//1-complete, 0-not
	}
}
﻿using System.Collections.Generic;

namespace Game.Systems.StorageSystem
{
	public sealed class GameProgressData
	{
		public List<RegularLevelData> regularLevels = new();

		public GameProgressData(int count)
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
}
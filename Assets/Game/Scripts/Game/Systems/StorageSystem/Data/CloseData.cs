using StarSmithGames.Core;

using System.Collections.Generic;

namespace Game.Systems.StorageSystem
{
	public struct CloseData
	{
		public int appCloseTimestampUTC;
		public int appCloseTimestampLocal;
		public bool isInterruptGameProcess;

		public string promptLevelFinishName;
		public Dictionary<string, object> promptLevelFinish;

		public bool IsPromptValid => !promptLevelFinishName.IsEmpty() && promptLevelFinish.Count != 0;
	}
}
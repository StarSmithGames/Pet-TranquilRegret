using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Managers.LevelManager
{
	[CreateAssetMenu(fileName = "LevelSettings", menuName = "Game/Level")]
    public class LevelSettings : ScriptableObject
    {
		public GameTime estimatePlatinaTime;
        [Space]
		public GameTime estimateGoldTime;
        [Space]
		public GameTime estimateSilverTime;
        [Space]
		public GameTime estimateCooperTime;

		[AssetSelector]
		public List<CountableGoalData> primaryGoals = new List<CountableGoalData>();
		[SerializeReference] public List<AbstractGoalData> secondaryGoals = new List<AbstractGoalData>();
	}

	[InlineProperty]
	[System.Serializable]
	public struct GameTime
	{
		public int TotalSeconds => Mathf.FloorToInt((float) new TimeSpan(0, minutes, seconds).TotalSeconds);

		public int minutes;
		public int seconds;
	}
}
using System;
using System.Collections.Generic;
using System.Linq;

using Game.Extensions;

using Sirenix.OdinInspector;

using StarSmithGames;
using StarSmithGames.Core;

using UnityEngine;

namespace Game.Systems.LevelSystem
{
	[InlineEditor]
	[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
		public int id;

		public string name;

		public SceneReference scene;

		[Min(0)]
		[InfoBox("@Time")]
		[SuffixLabel("s", true)]
		public int estimatedTime = 300;//5 minutes
		[Min(0)]
		public int penaltyTime = 15;
		[Min(0)]
		public int addTime = 60;
		[Min(0)]
		public int remainingTime = 30;//redZone
		[Space]
		public List<GoalItem> primaryGoals = new();
		//[SerializeReference] public List<AbstractGoalConfig> secondaryGoals = new List<AbstractGoalConfig>();

		private string Title => $"{id}_{name}";

		private string Time => $"Timer: {GetTime(estimatedTime)}\nPenalty: {GetTime(penaltyTime)}\nAdd: {GetTime(addTime)}\nRemainig: {GetTime(remainingTime)}";
		private string GetTime(int t)
		{
			return TimeExtensions.GetTimerFormat(TimeSpan.FromSeconds(t));

		}
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
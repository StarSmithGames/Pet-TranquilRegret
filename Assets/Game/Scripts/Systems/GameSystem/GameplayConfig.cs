using Game.Managers.LevelManager;

using Sirenix.OdinInspector;

using System.Collections.Generic;

using UnityEngine;

namespace Game.Systems.GameSystem
{
	[CreateAssetMenu(fileName = "GameplayConfig", menuName = "Game/GameplayConfig")]
	public class GameplayConfig : ScriptableObject
	{
		[ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "Title")]
		public List<LevelConfig> levels = new List<LevelConfig>();

		[Header("Audio")]
		public List<AudioClip> taps = new List<AudioClip>();
	}
}
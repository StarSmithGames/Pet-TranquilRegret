using Game.Character;

using Sirenix.OdinInspector;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Localization;

namespace Game.Systems.GameSystem
{
	[CreateAssetMenu(fileName = "GameplayConfig", menuName = "Game/GameplayConfig")]
	public class GameplayConfig : ScriptableObject
	{
		[ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "Title")]
		public List<LevelConfig> levels = new List<LevelConfig>();

		[Header("Audio")]
		public List<AudioClip> taps = new List<AudioClip>();

		public LocalizationSettins localizationSettins;

		[Header("Prefabs")]
		public AbstractCharacter characterPrefab;
	}

	[System.Serializable]
	public class LocalizationSettins
	{
		public List<Flag> flags = new List<Flag>();
	}


	[System.Serializable]
	public class Flag
	{
		public Sprite sprite;
		public Locale locale;
		public string country;
	}
}
using Game.Character;
using Game.Systems.LevelSystem;

using Sirenix.OdinInspector;

using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

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
	
		public LevelConfig GetLevelByScene(Scene scene)
		{
			return levels.Find((x) => x.scene.SceneName == scene.name);
		}
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
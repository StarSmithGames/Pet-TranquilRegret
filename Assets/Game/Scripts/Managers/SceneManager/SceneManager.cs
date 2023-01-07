using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;

namespace Game.Managers.SceneManager
{
    public class SceneManager
    {
		public bool IsCurrentSceneMenu => ScenePathes.First().Key == CurrentSceneName;
		public bool IsCurrentSceneLevel => !IsCurrentSceneMenu;

		public string CurrentSceneName { get; private set; }
		public string CurrentScenePath { get; private set; }

		public Dictionary<string, string> ScenePathes { get; private set; } = new Dictionary<string, string>();

		public SceneManager()
        {
			int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
			for (int i = 0; i < sceneCount; i++)
			{
				var s = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
				ScenePathes.Add(System.IO.Path.GetFileNameWithoutExtension(s), s);
			}

			ScenePathes.TryGetValue(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, out string value);

			UpdateScene();
		}

		private void UpdateScene()
		{
			CurrentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
			ScenePathes.TryGetValue(CurrentSceneName, out string value);
			CurrentScenePath = value;
		}
	}
}
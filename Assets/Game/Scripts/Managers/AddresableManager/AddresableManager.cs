using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Managers.AddresableManager
{
	public class AddresableManager
	{
		private bool clearPreviousScene;
		//private SceneInstance previousScene;

		public AddresableManager()
		{
			//Addressables.InitializeAsync().Completed += OnAddressableInitializCompleted;
		}

		public string GetSceneKey(string sceneName)
		{
			return $"Assets/Game/Scenes/Levels/{sceneName}/{sceneName}.unity";
		}
		public string GetSceneSettingsKey(string sceneName)
		{
			return $"Assets/Game/Scenes/Levels/{sceneName}/{sceneName}.asset";
		}

		//public AsyncOperationHandle<SceneInstance> LoadAddressableLevel(string key)
		//{
		//	if (clearPreviousScene)
		//	{
		//		Addressables.UnloadSceneAsync(previousScene).Completed += (asyncHandle) =>
		//		{
		//			clearPreviousScene = false;
		//			previousScene = new SceneInstance();
		//			Debug.LogError($"Unload scene {key} succefully");
		//		};
		//	}

		//	var operation = Addressables.LoadSceneAsync(key, LoadSceneMode.Single);
		//	operation.Completed += (asyncHandle) =>
		//	{
		//		clearPreviousScene = true;
		//		previousScene = asyncHandle.Result;
		//		Debug.LogError($"Load scene {key} succefully");
		//	};

		//	return operation;
		//}

		//private void OnAddressableInitializCompleted(AsyncOperationHandle<IResourceLocator> operationHandle)
		//{
		//	Debug.LogError($"OnAddressableInitializCompleted");
		//}
	}

	//[System.Serializable]
	//public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
	//{
	//	public AssetReferenceAudioClip(string guid) : base(guid) { }
	//}
}
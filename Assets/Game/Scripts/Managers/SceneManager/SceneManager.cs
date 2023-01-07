using Game.Managers.TransitionManager;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using Zenject;

namespace Game.Managers.SceneManager
{
	public class SceneManager
    {
		public bool IsCurrentSceneMenu => ScenePathes.First().Key == CurrentSceneName;
		public bool IsCurrentSceneLevel => !IsCurrentSceneMenu;

		public string CurrentSceneName { get; private set; }
		public string CurrentScenePath { get; private set; }

		public IProgressHandler CurrentProgressHandle { get; private set; }

		public Dictionary<string, string> ScenePathes { get; private set; } = new Dictionary<string, string>();

		private SignalBus signalBus;
		private AsyncManager.AsyncManager asyncManager;
		private TransitionManager.TransitionManager transitionManager;
		private InfinityLoading infinityLoading;

		public SceneManager(
			SignalBus signalBus,
			AsyncManager.AsyncManager asyncManager,
			TransitionManager.TransitionManager transitionManager,
			InfinityLoading infinityLoading)
        {
			this.signalBus = signalBus;
			this.asyncManager = asyncManager;
			this.transitionManager = transitionManager;
			this.infinityLoading = infinityLoading;

			int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
			for (int i = 0; i < sceneCount; i++)
			{
				var s = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
				ScenePathes.Add(System.IO.Path.GetFileNameWithoutExtension(s), s);
			}

			ScenePathes.TryGetValue(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, out string value);

			UpdateCurrentScene();
		}

		public void SwitchScene(string sceneName, bool allow = true, UnityAction callback = null)
		{
			asyncManager.StartCoroutine(LoadFromBuild(sceneName, allow, callback));
		}

		private IEnumerator LoadFromBuild(string sceneName, bool allow = true, UnityAction callback = null)
		{
			transitionManager.In();

			BuildProgressHandle handle = new BuildProgressHandle();
			CurrentProgressHandle = handle;

			//Load Scene
			handle.AsyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
			handle.AsyncOperation.allowSceneActivation = allow;

			if(allow == false)
			{
				infinityLoading.StartLoading(CurrentProgressHandle);
			}

			yield return handle.AsyncOperation;

			if (handle.AsyncOperation.isDone)
			{
				Debug.LogError("LOADED");

				UpdateCurrentScene();
				signalBus?.Fire(new SignalSceneChanged());
				callback?.Invoke();

				yield return new WaitWhile(() => transitionManager.IsInProccess);

				transitionManager.Out(() =>
				{
					signalBus?.Fire(new SignalSceneChangedLate());
				});
			}
			else
			{
				Debug.LogError("REJECT Scene no loaded");
			}
		}


		private void UpdateCurrentScene()
		{
			CurrentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
			ScenePathes.TryGetValue(CurrentSceneName, out string value);
			CurrentScenePath = value;
		}
	}
}
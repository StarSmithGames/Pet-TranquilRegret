using Game.Managers.GameManager;
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
	public class SceneManager : IInitializable
    {
		public bool IsCurrentSceneMenu => ScenePathes.First().Key == CurrentSceneName;
		public bool IsCurrentSceneLevel => !IsCurrentSceneMenu;

		public string CurrentSceneName { get; private set; }
		public string CurrentSceneNameImmediately => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		public string CurrentScenePath { get; private set; }

		public IProgressHandler CurrentProgressHandle { get; private set; }

		public Dictionary<string, string> ScenePathes { get; private set; } = new Dictionary<string, string>();

		private SignalBus signalBus;
		private AsyncManager.AsyncManager asyncManager;
		private TransitionManager.TransitionManager transitionManager;
		private InfinityLoading infinityLoading;
		private GameManager.GameManager gameManager;

		public SceneManager(
			SignalBus signalBus,
			AsyncManager.AsyncManager asyncManager,
			TransitionManager.TransitionManager transitionManager,
			InfinityLoading infinityLoading,
			GameManager.GameManager gameManager)
        {
			this.signalBus = signalBus;
			this.asyncManager = asyncManager;
			this.transitionManager = transitionManager;
			this.infinityLoading = infinityLoading;
			this.gameManager = gameManager;

			int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
			for (int i = 0; i < sceneCount; i++)
			{
				var s = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
				ScenePathes.Add(System.IO.Path.GetFileNameWithoutExtension(s), s);
			}

			ScenePathes.TryGetValue(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, out string value);

			UpdateCurrentScene();
		}

		public void Initialize()
		{
			if (IsCurrentSceneMenu)
			{
				gameManager.ChangeState(GameState.Menu);
			}
			else
			{
				gameManager.ChangeState(GameState.PreGameplay);
			}
		}

		public void SwitchScene(string sceneName, bool allow = true, UnityAction callback = null)
		{
			asyncManager.StartCoroutine(LoadFromBuild(sceneName, allow, callback));
		}

		private IEnumerator LoadFromBuild(string sceneName, bool allow = true, UnityAction callback = null)
		{
			gameManager.ChangeState(GameState.Loading);

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
				UpdateCurrentScene();
				signalBus?.Fire(new SignalSceneChanged());
				callback?.Invoke();

				yield return new WaitWhile(() => transitionManager.IsInProccess);

				transitionManager.Out(() =>
				{
					signalBus?.Fire(new SignalSceneChangedLate());
					gameManager.ChangeState(GameState.PreGameplay);
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
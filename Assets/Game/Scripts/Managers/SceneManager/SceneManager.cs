using Game.Managers.GameManager;
using Game.Managers.LevelManager;
using Game.Managers.TransitionManager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;

using Zenject;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Managers.SceneManager
{
	public class SceneManager : IInitializable
    {
		public bool IsCurrentSceneMenu => ScenePathesInBuild.First().Key == CurrentSceneName;
		public bool IsCurrentSceneLevel => !IsCurrentSceneMenu;

		public string CurrentSceneName { get; private set; }
		public string CurrentSceneNameImmediately => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		public string CurrentScenePath { get; private set; }

		public LevelSettings CurrentLevelSettings
		{
			private set => currentLevelSettins = value;
			get
			{
#if UNITY_EDITOR

				if (currentLevelSettins == null)
				{
					currentLevelSettins = AssetDatabase.LoadAssetAtPath<LevelSettings>(addresableManager.GetSceneSettingsKey(CurrentSceneNameImmediately));
				}
#endif
				return currentLevelSettins;
			}
		}
		private LevelSettings currentLevelSettins;

		public IProgressHandler CurrentProgressHandle { get; private set; }

		public Dictionary<string, string> ScenePathesInBuild { get; private set; } = new Dictionary<string, string>();
		private Dictionary<string, IResourceLocation> resourceLocations = new Dictionary<string, IResourceLocation>();

		private SignalBus signalBus;
		private AsyncManager.AsyncManager asyncManager;
		private TransitionManager.TransitionManager transitionManager;
		private InfinityLoading infinityLoading;
		private AddresableManager.AddresableManager addresableManager;
		private GameManager.GameManager gameManager;

		public SceneManager(
			SignalBus signalBus,
			AsyncManager.AsyncManager asyncManager,
			TransitionManager.TransitionManager transitionManager,
			InfinityLoading infinityLoading,
			AddresableManager.AddresableManager addresableManager,
			GameManager.GameManager gameManager)
        {
			this.signalBus = signalBus;
			this.asyncManager = asyncManager;
			this.transitionManager = transitionManager;
			this.infinityLoading = infinityLoading;
			this.addresableManager = addresableManager;
			this.gameManager = gameManager;

			int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
			for (int i = 0; i < sceneCount; i++)
			{
				var s = SceneUtility.GetScenePathByBuildIndex(i);
				ScenePathesInBuild.Add(System.IO.Path.GetFileNameWithoutExtension(s), s);
			}

			ScenePathesInBuild.TryGetValue(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, out string value);

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
			if(sceneName == "Menu")
			{
				asyncManager.StartCoroutine(LoadFromBuild(sceneName, allow, callback));
			}
			else
			{
				asyncManager.StartCoroutine(LoadFromAddresables(sceneName, allow, callback));
			}
		}

		private IEnumerator LoadFromBuild(string sceneName, bool allow = true, UnityAction callback = null)
		{
			gameManager.ChangeState(GameState.Loading);

			if (allow == false)
			{
				transitionManager.In();
			}

			BuildProgressHandle handle = new BuildProgressHandle();
			CurrentProgressHandle = handle;

			//Load Scene
			handle.AsyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
			handle.AsyncOperation.allowSceneActivation = allow;

			if(allow == false)
			{
				infinityLoading.StartLoading(handle);
			}

			yield return handle.AsyncOperation;

			if (handle.AsyncOperation.isDone)
			{
				UpdateCurrentScene();
				signalBus?.Fire(new SignalSceneChanged());
				callback?.Invoke();

				if (allow == false)
				{
					yield return new WaitWhile(() => transitionManager.IsInProccess);

					transitionManager.Out(() =>
					{
						signalBus?.Fire(new SignalSceneChangedLate());
						gameManager.ChangeState(GameState.PreGameplay);
					});
				}
			}
			else
			{
				Debug.LogError("REJECT Scene no loaded");
			}
		}

		private IEnumerator LoadFromAddresables(string sceneName, bool allow = true, UnityAction callback = null)
		{
			string addressableSceneKey = addresableManager.GetSceneKey(sceneName);
			string addressableSettingsKey = addresableManager.GetSceneSettingsKey(sceneName);

			gameManager.ChangeState(GameState.Loading);

			if (allow == false)
			{
				transitionManager.In();
			}

			//Asset
			Addressables.LoadAssetAsync<LevelSettings>(addressableSettingsKey).Completed += (handle) =>
			{
				if (handle.Status == AsyncOperationStatus.Succeeded)
				{
					CurrentLevelSettings = handle.Result;
				}
				else
				{
					Debug.LogError("ERROR ASSET NOT LOADED");
				}
			};

			//Scene
			AddressablesProgressHandle handle = new AddressablesProgressHandle();
			CurrentProgressHandle = handle;

			if (allow == false)
			{
				infinityLoading.StartLoading(handle);
			}

			IResourceLocation locate = null;
			if (!resourceLocations.TryGetValue(sceneName, out locate))
			{
				handle.LocationHandle = Addressables.LoadResourceLocationsAsync(addressableSceneKey);
				yield return handle.LocationHandle;

				if (handle.LocationHandle.IsDone &&
					handle.LocationHandle.Status == AsyncOperationStatus.Succeeded &&
					handle.LocationHandle.Result.Count > 0)
				{
					locate = handle.LocationHandle.Result[0];
					if (!resourceLocations.ContainsKey(sceneName))
					{
						resourceLocations.Add(sceneName, locate);
					}
					else
					{
						resourceLocations[sceneName] = locate;
					}
				}
			}

			if (locate != null)
			{
				handle.SceneHandle = Addressables.LoadSceneAsync(locate, LoadSceneMode.Single);
				yield return handle.SceneHandle;

				handle.DependenciesHandle = Addressables.DownloadDependenciesAsync(addressableSceneKey);
				yield return handle.DependenciesHandle;

				if (handle.SceneHandle.Status == AsyncOperationStatus.Succeeded)
				{
					UpdateCurrentScene();
					signalBus?.Fire(new SignalSceneChanged());
					callback?.Invoke();

					if (allow == false)
					{
						yield return new WaitWhile(() => transitionManager.IsInProccess);

						transitionManager.Out(() =>
						{
							signalBus?.Fire(new SignalSceneChangedLate());
							gameManager.ChangeState(GameState.PreGameplay);
						});
					}
				}
				else if (handle.SceneHandle.Status == AsyncOperationStatus.Failed)
				{
					Debug.LogError("REJECT Scene no loaded");
				}
				else
				{
					Debug.LogError("REJECT Scene no loaded");
				}
			}
			else
			{
				Debug.LogError("REJECT Scene no loaded");
			}
		}


		private void UpdateCurrentScene()
		{
			CurrentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
			ScenePathesInBuild.TryGetValue(CurrentSceneName, out string value);
			CurrentScenePath = value;
		}
	}
}
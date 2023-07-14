using Game.Managers.GameManager;
using StarSmithGames.Go.AsyncManager;
using StarSmithGames.Go.SceneManager;

using System;
using System.Collections;
using UnityEngine;

using Zenject;

namespace Game.Managers.TransitionManager
{
	public class TransitionManager
	{
		[Inject] private AsyncManager asyncManager;
		[Inject] private GameManager.GameManager gameManager;
		[Inject] private SceneManager sceneManager;

		private InfinityLoading infinityLoading;

		public TransitionManager(InfinityLoading infinityLoading)
		{
			this.infinityLoading = infinityLoading;
		}

		public void StartInfinityLoadingSceneAsync(int sceneInBuild, bool allow, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			asyncManager.StartCoroutine(LoaderBuild(sceneInBuild, allow, onShowed, onHided, callback));
		}

		private IEnumerator LoaderBuild(int sceneInBuild, bool allow, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			sceneManager.LoadSceneAsyncFromBuild(sceneInBuild, false);
			var buildHandler = sceneManager.ProgressHandler as BuildProgressHandler;

			infinityLoading
				.StartProgress(buildHandler)
				.Show(() =>
				{
					gameManager.ChangeState(GameState.Loading);
					onShowed?.Invoke();
				});
			yield return infinityLoading.WaitUntilDone();
			infinityLoading.Hide(onHided);
			yield return new WaitForSeconds(0.5f);

			//if (allow)
			//{
			//	buildHandler.AllowSceneActivation();
			//}

			callback?.Invoke();
		}


		public void StartInfinityLoadingFict(Action onShowed = null, Action onHided = null, Action callback = null)
		{
			asyncManager.StartCoroutine(LoaderFict(onShowed, onHided, callback));
		}

		private IEnumerator LoaderFict(Action onShowed = null, Action onHided = null, Action callback = null)
		{
			infinityLoading
				.StartProgress(65f)
				.Show(() =>
				{
					gameManager.ChangeState(GameState.Loading);
					onShowed?.Invoke();
				});
			yield return infinityLoading.WaitUntilDone();
			infinityLoading.Hide(onHided);
			yield return new WaitForSeconds(0.5f);
			callback?.Invoke();
		}
	}
}
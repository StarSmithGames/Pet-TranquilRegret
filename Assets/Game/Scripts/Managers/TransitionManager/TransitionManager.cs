using Game.Managers.GameManager;
using StarSmithGames.Go.SceneManager;
using StarSmithGames.IoC.AsyncManager;

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

		public void StartInfinityLoadingFict(FictProgressHandler fictProgressHandler, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			asyncManager.StartCoroutine(LoaderFict(fictProgressHandler, onShowed, onHided, callback));
		}

		public void StartInfinityLoading(Func<IProgressHandler> progressHandler, bool allow = true, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			asyncManager.StartCoroutine(LoaderProgress(progressHandler, allow, onShowed, onHided, callback));
		}

		private IEnumerator LoaderProgress(Func<IProgressHandler> progressHandler, bool allow = true, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			infinityLoading.Show(onShowed);
			yield return infinityLoading.WaitUntilProcessDone();
			yield return new WaitForSeconds(0.16f);
			var progress = progressHandler.Invoke();
			yield return Loop(progress);
			yield return progress.WaitUntilDone();
			if (allow)
			{
				infinityLoading.Hide(onHided);
			}
			else
			{
				yield return new WaitForSeconds(5f);
				infinityLoading.Hide(onHided);
			}
			callback?.Invoke();
		}

		private IEnumerator LoaderFict(FictProgressHandler fictProgressHandler, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			fictProgressHandler.speed = 65f;

			infinityLoading.Show(() =>
			{
				gameManager.ChangeState(GameState.Loading);
				onShowed?.Invoke();
			});
			yield return infinityLoading.WaitUntilProcessDone();
			yield return null;
			yield return fictProgressHandler.WaitUntilDone();
			infinityLoading.Hide(onHided);
			callback?.Invoke();
		}

		private IEnumerator Loop(IProgressHandler progress)
		{
			infinityLoading.progress.text = "0%";

			while (!progress.IsDone)
			{
				infinityLoading.progress.text = $"{progress.GetProgress() * 100}%";

				yield return null;
			}

			infinityLoading.progress.text = "100%";
		}
	}
}
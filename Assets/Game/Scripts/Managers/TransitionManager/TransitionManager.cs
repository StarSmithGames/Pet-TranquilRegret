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

		private InfinityLoading infinityLoading;

		public TransitionManager(InfinityLoading infinityLoading)
		{
			this.infinityLoading = infinityLoading;
		}

		public void StartInfinityLoadingFict(FictProgressHandler fictProgressHandler, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			asyncManager.StartCoroutine(LoaderFict(fictProgressHandler, onShowed, onHided, callback));
		}

		public void StartInfinityLoading(Func<IProgressHandler> progressHandler, float waitTime = 0f, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			asyncManager.StartCoroutine(LoaderProgress(progressHandler, waitTime, onShowed, onHided, callback));
		}

		private IEnumerator LoaderProgress(Func<IProgressHandler> progressHandler, float waitTime = 0f, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			infinityLoading.Show(onShowed);
			yield return infinityLoading.WaitUntilProcessDone();
			yield return new WaitForSeconds(0.16f);
			var progress = progressHandler.Invoke();
			yield return Loop(progress);
			yield return progress.WaitUntilDone();
			if (waitTime > 0f)
			{
				yield return new WaitForSeconds(waitTime);
			}
			infinityLoading.Hide(onHided);
			callback?.Invoke();
		}

		private IEnumerator LoaderFict(FictProgressHandler fictProgressHandler, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			fictProgressHandler.speed = 65f;

			infinityLoading.Show(() =>
			{
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
				infinityLoading.progress.text = $"{Math.Round(progress.GetProgress() * 100f)}%";

				yield return null;
			}

			infinityLoading.progress.text = "100%";
		}
	}
}
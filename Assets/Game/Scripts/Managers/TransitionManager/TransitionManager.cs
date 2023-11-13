using Game.Managers.GameManager;
using StarSmithGames.Go.SceneManager;
using StarSmithGames.IoC.AsyncManager;

using System;
using System.Collections;
using UnityEditor;

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

		public void StartInfinityLoading(Transition transition, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			asyncManager.StartCoroutine(LoaderProgress(transition, onShowed, onHided, callback));
		}

		private IEnumerator LoaderProgress(Transition transition, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			infinityLoading.SetProgress(0);
			infinityLoading.Show(onShowed);
			yield return infinityLoading.WaitUntilProcessDone();
			yield return new WaitForSeconds(0.16f);
			transition.Invoke();
			yield return Loop(transition.Progress);
			yield return transition.WaitUntilDone();
			yield return new WaitForSeconds(0.16f);
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
			while (!progress.IsDone)
			{
				infinityLoading.SetProgress(progress.GetProgress());

				yield return null;
			}

			infinityLoading.SetProgress(100);
		}
	}

	public class Transition
	{
		public IProgressHandler Progress => progress;
		private IProgressHandler progress;

		private event Action onProgressCompleted;
		private Func<IProgressHandler> progressHandler;
		private bool isAllowed;

		public Transition(Func<IProgressHandler> progressHandler, bool allow = true, Action onProgressCompleted = null)
		{
			this.progressHandler = progressHandler;
			this.isAllowed = allow;
			this.onProgressCompleted = onProgressCompleted;
		}

		public void Invoke()
		{
			progress = progressHandler.Invoke();
		}

		public void Allow()
		{
			isAllowed = true;
		}

		public IEnumerator WaitUntilDone()
		{
			yield return progress.WaitUntilDone();
			onProgressCompleted?.Invoke();
			yield return new WaitUntil(() => isAllowed);
		}
	}
}
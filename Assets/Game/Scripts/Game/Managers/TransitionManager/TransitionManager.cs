using Cysharp.Threading.Tasks;
using StarSmithGames.Go.SceneManager;
using StarSmithGames.IoC.AsyncManager;

using System;
using System.Collections;
using UnityEngine;

namespace Game.Managers.TransitionManager
{
	public class TransitionManager
	{
		private InfinityLoading _infinityLoading;
		private AsyncManager _asyncManager;

		public TransitionManager(
			InfinityLoading infinityLoading,
			AsyncManager asyncManager
			)
		{
			_infinityLoading = infinityLoading;
			_asyncManager = asyncManager;
		}

		public void StartInfinityLoadingFict(FictProgressHandler fictProgressHandler, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			_asyncManager.StartCoroutine(LoaderFict(fictProgressHandler, onShowed, onHided, callback));
		}

		public void StartInfinityLoadingAsync(Transition transition, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			LoaderProgressAsync( transition, onShowed, onHided, callback ).Forget();
		}
		
		public void StartInfinityLoading(Transition transition, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			_asyncManager.StartCoroutine( LoaderProgress( transition, onShowed, onHided, callback ) );
		}
		
		private async UniTask LoaderProgressAsync(Transition transition, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			_infinityLoading.SetProgress(0);
			_infinityLoading.Show(onShowed);
			await _infinityLoading.WaitUntilProcessDone();
			await UniTask.WaitForSeconds(0.16f);
			transition.Invoke();
			await Loop(transition.Progress);
			await transition.WaitUntilDone();
			await UniTask.WaitForSeconds(0.16f);
			_infinityLoading.Hide(onHided);
			callback?.Invoke();
		}

		private IEnumerator LoaderProgress(Transition transition, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			_infinityLoading.SetProgress(0);
			_infinityLoading.Show(onShowed);
			yield return _infinityLoading.WaitUntilProcessDone();
			yield return new WaitForSeconds(0.16f);
			transition.Invoke();
			yield return Loop(transition.Progress);
			yield return transition.WaitUntilDone();
			yield return new WaitForSeconds(0.16f);
			_infinityLoading.Hide(onHided);
			callback?.Invoke();
		}

		private IEnumerator LoaderFict(FictProgressHandler fictProgressHandler, Action onShowed = null, Action onHided = null, Action callback = null)
		{
			fictProgressHandler.speed = 65f;

			_infinityLoading.Show(() =>
			{
				onShowed?.Invoke();
			});
			yield return _infinityLoading.WaitUntilProcessDone();
			yield return null;
			yield return fictProgressHandler.WaitUntilDone();
			_infinityLoading.Hide(onHided);
			callback?.Invoke();
		}

		private IEnumerator Loop(IProgressHandler progress)
		{
			while (!progress.IsDone)
			{
				_infinityLoading.SetProgress(progress.GetProgress());

				yield return null;
			}

			_infinityLoading.SetProgress(100);
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
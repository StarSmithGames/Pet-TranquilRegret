using StarSmithGames.Go;
using StarSmithGames.Go.SceneManager;

using System.Collections;
using UnityEngine;

namespace Game.Managers.TransitionManager
{
	public class InfinityLoading : ViewBase
	{
		public bool IsLoading { get; private set; } = false;

		private IProgressHandler progressHandler;

		private InfinityLoadingSettings settings;

		public InfinityLoading(InfinityLoadingSettings settings)
		{
			this.settings = settings;
		}

		public InfinityLoading StartProgress(float speed = 50f)
		{
			var fictProgressHandle = new FictProgressHandler();
			fictProgressHandle.speed = speed;

			progressHandler = fictProgressHandle;

			return this;
		}

		public InfinityLoading StartProgress(BuildProgressHandler buildProgressHandler)
		{
			progressHandler = buildProgressHandler;

			return this;
		}

		public IEnumerator WaitUntilDone()
		{
			if(progressHandler != null)
			{
				yield return new WaitUntil(() => progressHandler.IsDone);
			}
		}


		//public void StartLoading(AddressablesProgressHandle progressHandler, UnityAction callback = null)
		//{
		//	asyncManager.StartCoroutine(Loading(progressHandler, callback));
		//}

		//private IEnumerator Loading(BuildProgressHandle progressHandler, UnityAction callback = null)
		//{
		//	IsLoading = true;

		//	float targetValue;
		//	float currentValue = 0f;

		//	while (IsLoading)
		//	{
		//		targetValue = progressHandler.GetProgressPercent() / 0.9f;

		//		currentValue = Mathf.MoveTowards(currentValue, targetValue, settings.progressAnimationMultiplier * Time.deltaTime);
		//		//Progress.text = $"{Mathf.Round(currentValue * 100f)}%";

		//		if (Mathf.Approximately(currentValue, 1))
		//		{
		//			//end progress
		//			IsLoading = false;

		//			progressHandler.AllowSceneActivation();

		//			callback?.Invoke();
		//			yield return null;
		//		}

		//		yield return null;
		//	}
		//}

		//private IEnumerator Loading(AddressablesProgressHandle progressHandler, UnityAction callback = null)
		//{
		//	IsLoading = true;

		//	float targetValue;
		//	float currentValue = 0f;

		//	while (IsLoading)
		//	{
		//		targetValue = progressHandler.GetProgressPercent() / 0.9f;

		//		currentValue = Mathf.MoveTowards(currentValue, targetValue, settings.progressAnimationMultiplier * Time.deltaTime);
		//		//Progress.text = $"{Mathf.Round(currentValue * 100f)}%";

		//		if (Mathf.Approximately(currentValue, 1))
		//		{
		//			//end progress
		//			IsLoading = false;

		//			callback?.Invoke();
		//			yield return null;
		//		}

		//		yield return null;
		//	}
		//}
	}

	[System.Serializable]
	public class InfinityLoadingSettings
	{
		[Min(0.1f)]
		public float progressAnimationMultiplier;
	}
}
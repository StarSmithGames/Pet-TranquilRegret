using Game.Managers.SceneManager;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Managers.TransitionManager
{
	public class InfinityLoading
	{
		public bool IsLoading { get; private set; } = false;

		private InfinityLoadingSettings settings;
		private AsyncManager.AsyncManager asyncManager;

		public InfinityLoading(InfinityLoadingSettings settings, AsyncManager.AsyncManager asyncManager)
		{
			this.settings = settings;
			this.asyncManager = asyncManager;
		}

		public void StartLoading(BuildProgressHandle progressHandler, UnityAction callback = null)
		{
			asyncManager.StartCoroutine(Loading(progressHandler, callback));
		}

		public void StartLoading(AddressablesProgressHandle progressHandler, UnityAction callback = null)
		{
			asyncManager.StartCoroutine(Loading(progressHandler, callback));
		}

		private IEnumerator Loading(BuildProgressHandle progressHandler, UnityAction callback = null)
		{
			IsLoading = true;

			float targetValue;
			float currentValue = 0f;

			while (IsLoading)
			{
				targetValue = progressHandler.GetProgressPercent() / 0.9f;

				currentValue = Mathf.MoveTowards(currentValue, targetValue, settings.progressAnimationMultiplier * Time.deltaTime);
				//Progress.text = $"{Mathf.Round(currentValue * 100f)}%";

				if (Mathf.Approximately(currentValue, 1))
				{
					//end progress
					IsLoading = false;

					progressHandler.AllowSceneActivation();

					callback?.Invoke();
					yield return null;
				}

				yield return null;
			}
		}

		private IEnumerator Loading(AddressablesProgressHandle progressHandler, UnityAction callback = null)
		{
			IsLoading = true;

			float targetValue;
			float currentValue = 0f;

			while (IsLoading)
			{
				targetValue = progressHandler.GetProgressPercent() / 0.9f;

				currentValue = Mathf.MoveTowards(currentValue, targetValue, settings.progressAnimationMultiplier * Time.deltaTime);
				//Progress.text = $"{Mathf.Round(currentValue * 100f)}%";

				if (Mathf.Approximately(currentValue, 1))
				{
					//end progress
					IsLoading = false;

					callback?.Invoke();
					yield return null;
				}

				yield return null;
			}
		}
	}

	[System.Serializable]
	public class InfinityLoadingSettings
	{
		[Min(0.1f)]
		public float progressAnimationMultiplier;
	}
}
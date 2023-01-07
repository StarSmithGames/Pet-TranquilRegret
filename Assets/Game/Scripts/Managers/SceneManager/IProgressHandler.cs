using UnityEngine;

namespace Game.Managers.SceneManager
{
	public interface IProgressHandler
	{
		bool IsAllowed { get; }

		float GetProgressPercent();
		void AllowSceneActivation();
	}

	public class BuildProgressHandle : IProgressHandler
	{
		public AsyncOperation AsyncOperation = null;

		public bool IsAllowed => AsyncOperation?.allowSceneActivation ?? true;

		public float GetProgressPercent()
		{
			return AsyncOperation?.progress ?? 0f;
		}

		public void AllowSceneActivation()
		{
			AsyncOperation.allowSceneActivation = true;
		}
	}
}
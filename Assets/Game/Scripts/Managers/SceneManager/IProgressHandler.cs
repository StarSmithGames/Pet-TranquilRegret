using System.Collections.Generic;

using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Game.Managers.SceneManager
{
	public interface IProgressHandler
	{
		float GetProgressPercent();
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

	public class AddressablesProgressHandle : IProgressHandler
	{
		public AsyncOperationHandle<IList<IResourceLocation>> LocationHandle;
		public AsyncOperationHandle SceneHandle;
		public AsyncOperationHandle DependenciesHandle;

		public float GetProgressPercent()
		{
			var p1 = LocationHandle.IsValid() ? LocationHandle.PercentComplete : 0f;
			var p2 = SceneHandle.IsValid() ? SceneHandle.PercentComplete : 0f;
			var p3 = DependenciesHandle.IsValid() ? DependenciesHandle.PercentComplete : 0f;
			return (p1 + p2 + p3) / 3f;
		}
	}
}
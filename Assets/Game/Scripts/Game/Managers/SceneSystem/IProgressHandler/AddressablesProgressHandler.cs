using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Game.Systems.SceneSystem
{
    public sealed class AddressablesProgressHandler : IProgressHandler
    {
		public bool IsDone => sceneHandle.IsValid() ? sceneHandle.Status == AsyncOperationStatus.Succeeded : false;

		public AsyncOperationHandle< IList< IResourceLocation > > locationHandle;
		public AsyncOperationHandle< SceneInstance > sceneHandle;
		public AsyncOperationHandle dependenciesHandle;

		public float GetProgress()
		{
			var p1 = locationHandle.IsValid() ? locationHandle.PercentComplete : 0f;
			var p2 = sceneHandle.IsValid() ? sceneHandle.PercentComplete : 0f;
			var p3 = dependenciesHandle.IsValid() ? dependenciesHandle.PercentComplete : 0f;
			return (p1 + p2 + p3) / 3f;
		}
    }
}
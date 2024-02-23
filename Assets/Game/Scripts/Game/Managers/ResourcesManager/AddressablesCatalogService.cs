using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Managers.AssetManager
{
	public sealed class AddressablesCatalogService
	{
		private string _currentCdn = "";
		
		private IEnumerator LoadCatalogAsync(Action success, Action<AsyncOperationHandle> fail)
        {
            // LogSystem.Debug($"[Addressables] Load catalog start");

            var filename = $"catalog_{_currentCdn.ToLower()}.json";

            var catalog = $"{Application.streamingAssetsPath}/{filename}";

            // LogSystem.Debug($"[Addressables] Clear Resource Locators");

            Addressables.ClearResourceLocators();

            // LogSystem.Debug($"[Addressables] Load catalog from {filename}");

            var load = Addressables.LoadContentCatalogAsync(catalog);

            yield return load;
            yield return CheckDoneAsync(load, fail);

            // LogSystem.Debug($"[Addressables] Load end catalog with result {load.Status}");

            List<string> catalogsToUpdate = new List<string>();
            AsyncOperationHandle<List<string>> checkForUpdateHandle = Addressables.CheckForCatalogUpdates(false);
            checkForUpdateHandle.Completed += op =>
            {
                catalogsToUpdate.AddRange(op.Result);
            };

            yield return checkForUpdateHandle;
            yield return CheckDoneAsync(load, fail);

            // LogSystem.Debug($"[Addressables] End check catalog {checkForUpdateHandle.Status}");
            // catalogsToUpdate.ForEach(x => LogSystem.Debug($"[Addressables] catalog: {x}"));

            if (catalogsToUpdate.Count > 0)
            {
                // LogSystem.Debug($"[Addressables] Start UpdateCatalogs");
                AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate, false);
                yield return updateHandle;
                yield return CheckDoneAsync(load, fail);

                // LogSystem.Debug($"[Addressables] End UpdateCatalogs {updateHandle.Status}");
                Addressables.Release(updateHandle);
                // LogSystem.Debug($"[Addressables] Release UpdateCatalogs");
            }

            Addressables.Release(checkForUpdateHandle);

            success.Invoke();
        }
		
		private IEnumerator CheckDoneAsync(AsyncOperationHandle handle, Action<AsyncOperationHandle> failCallback)
		{
			if (!handle.IsDone || handle.Status != AsyncOperationStatus.Succeeded)
			{
				failCallback?.Invoke(handle);
				yield break;
			}
		}
	}
}
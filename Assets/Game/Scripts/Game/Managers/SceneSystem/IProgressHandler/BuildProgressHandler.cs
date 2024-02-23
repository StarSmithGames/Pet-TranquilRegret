using UnityEngine;

namespace Game.Systems.SceneSystem
{
    public sealed class BuildProgressHandler : IProgressHandler
    {
        public bool IsDone => asyncOperation?.isDone ?? true;
        public bool IsAllowed => asyncOperation?.allowSceneActivation ?? true;

        public AsyncOperation asyncOperation = null;

        public void AllowSceneActivation()
        {
            asyncOperation.allowSceneActivation = true;
        }
        
        public float GetProgress() => asyncOperation?.progress ?? 0f;
    }
}
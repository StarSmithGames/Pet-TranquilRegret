using UnityEngine;

namespace Game.Systems.SceneSystem
{
    public sealed class FictProgressHandler : IProgressHandler
    {
        public bool IsDone
        {
            get
            {
                Tick();
                return progress == 1f;
            }
        }

        public float speed = 69f;
        private float progress = 0;

        private void Tick()
        {
            var delta = Time.deltaTime * speed * 0.01f;
            progress += delta;
            progress = Mathf.Clamp01(progress);
        }

        public float GetProgress() => progress;
    }
}
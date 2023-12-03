using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Systems.NavigationSystem
{
    public class UIDynamicJoystick : UIJoystick
    {
        public bool IsEnable => canvasGroup.alpha == 1f;

		public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

        [SerializeField] private float moveThreshold = 1;
        [SerializeField] private CanvasGroup canvasGroup;

        protected override void Start()
        {
            MoveThreshold = moveThreshold;
            base.Start();
            background.gameObject.SetActive(false);
            canvasGroup.alpha = 1f;
		}

        public override void OnPointerDown(PointerEventData eventData)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            background.gameObject.SetActive(false);
            base.OnPointerUp(eventData);
        }

        protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            if (magnitude > moveThreshold)
            {
                Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
                background.anchoredPosition += difference;
            }
            base.HandleInput(magnitude, normalised, radius, cam);
        }

        [Button]
        public void Enable(bool trigger)
        {
            canvasGroup.alpha = trigger ? 1f : 0f;
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;

			handle.anchorMax = center;
			handle.anchorMin = center;
			handle.pivot = center;
			handle.anchoredPosition = Vector2.zero;

			background.anchorMax = Vector2.zero;
			background.anchorMin = Vector2.zero;
			background.pivot = center;
		}
	}
}
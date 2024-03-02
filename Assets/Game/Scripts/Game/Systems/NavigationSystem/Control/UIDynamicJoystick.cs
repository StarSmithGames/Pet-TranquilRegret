using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Game.Systems.NavigationSystem
{
    public class UIDynamicJoystick : UIJoystick
    {
        public bool IsEnable => canvasGroup.alpha == 1f;

        public bool isMovingOut;
        public float moveThreshold = 1;
        public CanvasGroup canvasGroup;

        protected override void Start()
        {
            base.Start();
            background.gameObject.SetActive(false);
            canvasGroup.alpha = 1f;
		}
        
        public void Enable(bool trigger)
        {
	        canvasGroup.alpha = trigger ? 1f : 0f;
        }

        public override void OnBeginDrag( PointerEventData eventData )
        {
	        base.OnBeginDrag(eventData);
	        
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
        }

        public override void OnEndDrag( PointerEventData eventData )
        {
	        background.gameObject.SetActive(false);
	        
	        base.OnEndDrag(eventData);
        }

        protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            if ( isMovingOut && magnitude > moveThreshold)
            {
                Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
                background.anchoredPosition += difference;
            }
            base.HandleInput(magnitude, normalised, radius, cam);
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
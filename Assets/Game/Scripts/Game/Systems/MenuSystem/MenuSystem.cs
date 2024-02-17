using Game.Managers.SwipeManager;
using Game.Systems.CameraSystem;
using Game.Systems.InfinityRoadSystem;
using Game.UI;
using UnityEngine;
using Zenject;

namespace Game.Systems.MenuSystem
{
	public sealed class MenuSystem : MonoBehaviour
	{
		public VerticalCameraSettings VerticalCameraSettings;
		public UIRootMenu UIRootMenu;
		public RoadMap RoadMap;
		
		private VerticalCamera _verticalCamera;
		
		private SwipeManager _swipeManager;
		
		[ Inject ]
		private void Construct(
			SwipeManager swipeManager
			)
		{
			_swipeManager = swipeManager;
		}

		private void Awake()
		{
			_verticalCamera = new( VerticalCameraSettings, UIRootMenu.camera, UIRootMenu.transform, RoadMap);
			
			_swipeManager.OnSwipeDetected += SwipeDetectedHandler;
		}

		public void OnDestroy()
		{
			_swipeManager.OnSwipeDetected -= SwipeDetectedHandler;
		}
		
		private void SwipeDetectedHandler( Swipe swipeDirection, Vector2 swipeVelocity )
		{
			_verticalCamera.Swipe( swipeDirection, swipeVelocity );
		}
	}
}
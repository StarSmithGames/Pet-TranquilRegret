using Game.Extensions;
using Game.Managers.SwipeManager;
using Game.Systems.InfinityRoadSystem;
using Game.Systems.LevelSystem;
using Game.Systems.StorageSystem;
using Game.Systems.UISystem;
using Game.UI;
using UnityEngine;
using Zenject;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Systems.MenuSystem
{
#if UNITY_EDITOR
	[ ExecuteAlways ]
#endif
	public sealed class MenuSystem : MonoBehaviour
	{
		public RoadCamera RoadCamera;
		public RoadClouds RoadClouds;
		public RoadBackground RoadBackground;
		public RoadPin RoadPin;
		public RoadMap RoadMap;
		[ Space ]
		public Vector3 StartPoint = new( 0, 95, 0);

		private LevelDialog _levelDialog;
		private GameProgress _gameProgress;

		private UIRootMenu _uiRootMenu;
		private SwipeManager _swipeManager;
		private StorageSystem.StorageSystem _storageSystem;
		private LevelRegularService _levelRegularService;
		
		[ Inject ]
		private void Construct(
			UIRootMenu uiRootMenu,
			SwipeManager swipeManager,
			StorageSystem.StorageSystem storageSystem,
			LevelRegularService levelRegularService
			)
		{
			_uiRootMenu = uiRootMenu;
			_swipeManager = swipeManager;
			_storageSystem = storageSystem;
			_levelRegularService = levelRegularService;
			
			_gameProgress = _storageSystem.GamePlayData.Storage.GameProgress.GetData();
			
			_uiRootMenu.ApplyCamera( RoadCamera.Camera );

			Initialize();
		}

		private void Initialize()
		{
			// RoadClouds.SetLerp(0);
			
			StartPoint.y -= _uiRootMenu.FrontCanvas.GetWorldHeight();
			StartPoint.y -= _uiRootMenu.NavigationCanvas.GetWorldHeight();

			RoadBackground.Refresh( StartPoint );
			RoadCamera.SetBounds( RoadBackground.GetSprites().CalculateBounds(), StartPoint );

			RoadMap.SetData( _storageSystem.GameFastData.LastRegularLevelIndex, _gameProgress.GetCurrentRegularIndex() );
			RoadMap.Initialize( RoadPin, RoadCamera.Camera );
			OpenRoadMap();

			RoadMap.OnPinDestinated += ShowLevelWindow;
			_swipeManager.OnSwipeDetected += SwipeDetectedHandler;
		}

		public void OnDestroy()
		{
			if ( _swipeManager != null )
			{
				_swipeManager.OnSwipeDetected -= SwipeDetectedHandler;
			}
		}

		private void Update()
		{
			RoadBackground.Refresh( StartPoint );
			RoadCamera.SetBounds( RoadBackground.GetSprites().CalculateBounds(), StartPoint );

			if ( !Application.isPlaying )
			{
				RoadCamera.SetPosition( StartPoint );
			}


			// Vector2 pos = RoadCamera.transform.position;
			// float lerp = Mathf.InverseLerp( RoadClouds.GetWorldStartPoint().y, RoadClouds.GetWorldEndPoint().y, pos.y );
			// RoadClouds.SetLerp( lerp );
		}
		
		private void OpenRoadMap()
		{
			// if ( _storageSystem.GameFastData.IsFirstTime )
			// {
			// 	RoadPinPrefab.DoMoveFirstTime( connections.First().GetPoints() );
			// }
			// else
			{
				Vector3 position = RoadMap.GetLastPosition();
				RoadCamera.SetPosition(position);
				RoadPin.transform.position = position;
			}
		}
		
		private void ShowLevelWindow()
		{
			_levelDialog = _uiRootMenu.DialogAggregator.CreateIfNotExist< LevelDialog >();
			_levelDialog.onStartClicked += LevelStartClickedHandler;
			_levelDialog.onClosed += LevelDialogClosedHandler;
			_levelDialog.SetLevel( _levelRegularService.GetLevelConfig( RoadMap.LastIndex + 1 ), _gameProgress.regularLevels[ RoadMap.LastIndex ] );
			_levelDialog.Show();
		}
		
		private void LevelStartClickedHandler()
		{
			LevelDialogClosedHandler();
			_storageSystem.GameFastData.LastRegularLevelIndex = RoadMap.LastIndex;
		}

		private void LevelDialogClosedHandler()
		{
			_levelDialog.onStartClicked -= LevelStartClickedHandler;
			_levelDialog.onClosed -= LevelDialogClosedHandler;
		}

		private void SwipeDetectedHandler( Swipe swipeDirection, Vector2 swipeVelocity )
		{
			RoadCamera.Swipe( swipeDirection, swipeVelocity );
		}

		
// #if UNITY_EDITOR
// 		[Button(DirtyOnClick = true)]
// 		private void Refresh()
// 		{
// 			levels = levelsContent.GetComponentsInChildren<UIRoadLevel>().ToList();
// 			connections = levelsContent.GetComponentsInChildren<LevelConnection>().ToList();
// 		}
//
// 		private void OnDrawGizmosSelected()
// 		{
// 			if (cloudsSettings.isFromTopCamera)
// 			{
// 				// cloudsSettings.cloudsEndPoint = verticalCamera.TopPointCamera;
// 				EditorUtility.SetDirty(gameObject);
// 			}
//
// 			Gizmos.color = Color.green;
// 			Gizmos.DrawSphere(cloudsSettings.GetWorldStartPoint(), 5f);
// 			Gizmos.DrawSphere(cloudsSettings.GetWorldEndPoint(), 5f);
//
//
// 			BackgroundRefresh();
// 		}
// #endif
	}
}
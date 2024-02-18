using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.InfinityRoadSystem
{
	public sealed class RoadMap : MonoBehaviour, IDisposable
	{
		public event Action OnPinDestinated;
		
		public bool IsInProcess { get; private set; }
		public int LastIndex { get; private set; }
		public int CurrentIndex { get; private set; }

		[Header("Levels")]
		public List<UIRoadLevel> levels = new();
		public List<LevelConnection> connections = new();

		private RoadPin _roadPin;
		
		public void SetData(
			int lastIndex,
			int currentIndex
			)
		{
			LastIndex = lastIndex;
			CurrentIndex = currentIndex;
		}
		
		public void Initialize(
			RoadPin RoadPin,
			Camera camera
			)
		{
			_roadPin = RoadPin;
			
			RefreshLevels();
			
			void RefreshLevels()
			{
				for (int i = 0; i < levels.Count; i++)
				{
					UIRoadLevel level = levels[ i ];
					level.Enable( i <= CurrentIndex );
					level.EnableStars( 0 );
					level.SetCamera( camera );
					level.OnButtonClicked += LevelButtonClickedHandler;
				}
			}
		}
		
		public void Dispose()
		{
			for (int i = 0; i < levels.Count; i++)
			{
				levels[ i ].OnButtonClicked -= LevelButtonClickedHandler;
			}
		}
		
		public Vector3 GetLastPosition()
		{
			return levels[ LastIndex ].transform.position;
		}

		private void LevelButtonClickedHandler(UIRoadLevel level)
		{
			if ( _roadPin.IsInProcess ) return;
			
			_roadPin.Break();

			if (level.IsEnable)
			{
				int index = levels.IndexOf(level);
				int diff = index - LastIndex;
				LastIndex = index;

				if (diff != 0)
				{
					IsInProcess = true;

					if (diff == 1)//up on 1
					{
						var connection = connections[levels.IndexOf(level)];
						var points = connection.GetPoints();

						_roadPin.DoMove( points, OnPinDestinated );
					}
					else//down or up
					{
						_roadPin.DoMoveInstant( level.transform.position, OnPinDestinated  );
					}
				}
				else
				{
					OnPinDestinated?.Invoke();
				}
			}
			//else
			//{
			//	//disabled
			//}
		}
	}


}
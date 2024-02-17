using Cysharp.Threading.Tasks;
using DG.Tweening;

using Game.Systems.LevelSystem;
using Game.UI;

using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;



using UnityEngine;

using Zenject;

namespace Game.Systems.InfinityRoadSystem
{
	public sealed class RoadMap : MonoBehaviour
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
			RoadPin RoadPin
			)
		{
			_roadPin = RoadPin;
			
			RefreshLevels();
			
			void RefreshLevels()
			{
				for (int i = 0; i < levels.Count; i++)
				{
					levels[ i ].Enable( i <= CurrentIndex );
					levels[ i ].EnableStars( 0 );
					levels[ i ].onClicked += LevelClickedHandler;
				}
			}
		}

		public Vector3 GetLastPosition()
		{
			return levels[ LastIndex ].transform.position;
		}

		private void LevelClickedHandler(UIRoadLevel level)
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
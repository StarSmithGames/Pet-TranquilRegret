using Sirenix.OdinInspector;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Game.Systems.InfinityRoadSystem
{
#if UNITY_EDITOR
	[ExecuteAlways]
#endif
	public class RoadClouds : MonoBehaviour
	{
		public CloudsSettings Settings;
	    public TMPro.TextMeshPro title;
		public List<Transform> left = new();
		public List<Transform> right = new();

#if UNITY_EDITOR
		[Range(0f, 1f)]
		public float lerp = 0f;
		public bool isDebug = true;
#endif
		[Space]
		[ReadOnly] public Positions lefts;
		[ReadOnly] public Positions rights;

#if UNITY_EDITOR
		private void Update()
		{
			if (Application.isPlaying) return;

			if (isDebug)
			{
				SetLerp(lerp);
			}
		}
#endif

		public void SetLerp(float lerp)
		{
			for (int i = 0; i < left.Count; i++)
			{
				left[i].localPosition = Vector3.Lerp(lefts.endPositions[i], lefts.startPositions[i], lerp);
			}

			for (int i = 0; i < right.Count; i++)
			{
				right[i].localPosition = Vector3.Lerp(rights.endPositions[i], rights.startPositions[i], lerp);
			}

			title.alpha = Mathf.InverseLerp(0.5f, 1f, lerp);
		}
		
		public Vector2 GetWorldStartPoint()
		{
			if ( Settings.isFromEndPoint)
			{
				return Settings.cloudsEndPoint - Settings.cloudsStartPoint;
			}

			return Settings.cloudsStartPoint;
		}

		public Vector2 GetWorldEndPoint()
		{
			return Settings.cloudsEndPoint;
		}

#if UNITY_EDITOR
		[Button(DirtyOnClick = true)]
		private void SetStart()
		{
			lefts.startPositions.Clear();
			rights.startPositions.Clear();

			lefts.startPositions.AddRange(left.Select((x) => (Vector2)x.localPosition));
			rights.startPositions.AddRange(right.Select((x) => (Vector2)x.localPosition));
		}

		[Button(DirtyOnClick = true)]
		private void SetEnd()
		{
			lefts.endPositions.Clear();
			rights.endPositions.Clear();

			lefts.endPositions.AddRange(left.Select((x) => (Vector2)x.localPosition));
			rights.endPositions.AddRange(right.Select((x) => (Vector2)x.localPosition));
		}
#endif
		[System.Serializable]
		public class Positions
		{
			public List<Vector2> startPositions = new();
			public List<Vector2> endPositions = new();
		}
	}
	
	[System.Serializable]
	public class CloudsSettings
	{
		public Vector2 cloudsStartPoint;
		public Vector2 cloudsEndPoint;
		public bool isFromTopCamera = true;
		public bool isFromEndPoint = true;
	}
}
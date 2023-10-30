using BansheeGz.BGSpline.Curve;
using System.Linq;

using UnityEngine;

namespace Game.Systems.InfinityRoadSystem
{
	[ExecuteInEditMode]
	public class LevelConnection : MonoBehaviour
	{
		public BGCurve Curve => curve;

		[SerializeField] private BGCurve curve;
		public LineRenderer LineRenderer => lineRenderer;
		[SerializeField] private LineRenderer lineRenderer;
		[Space]
		[SerializeField] private bool snap = true;
		[SerializeField] private Vector3 offsetLast;
		[SerializeField] private Vector3 offsetNext;

#if UNITY_EDITOR
		private void Update()
		{
			if (Application.isPlaying) return;

			if (curve == null)
			{
				curve = GetComponent<BGCurve>();
			}

			if (lineRenderer == null)
			{
				lineRenderer = GetComponent<LineRenderer>();
			}

			if (snap)
			{
				int index = transform.GetSiblingIndex();
				UIRoadLevel lastLevel = index - 1 >= 0 && index - 1 < transform.parent.childCount ? transform.parent.GetChild(index - 1).gameObject.GetComponent<UIRoadLevel>() : null;
				UIRoadLevel nextLevel = index + 1 >= 0 && index + 1 < transform.parent.childCount ? transform.parent.GetChild(index + 1).gameObject.GetComponent<UIRoadLevel>() : null;

				if (lastLevel != null)
				{
					curve.Points.First().PositionWorld = offsetLast + lastLevel.transform.position;
				}
				if (nextLevel != null)
				{
					curve.Points.Last().PositionWorld = offsetNext + nextLevel.transform.position;
				}

				gameObject.name = $"Conection";
			}
		}
#endif

		public Vector3[] GetPoints()
		{
			Vector3[] points = new Vector3[LineRenderer.positionCount];
			for (int i = 0; i < LineRenderer.positionCount; i++)
			{
				points[i] = transform.TransformPoint(LineRenderer.GetPosition(i));
			}

			return points;
		}

	}
}
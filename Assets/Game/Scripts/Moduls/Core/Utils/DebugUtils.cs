using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugUtils
{
    public static void DrawPath(Vector3[] points)
    {
		for (int i = 0; i < points.Length - 1; i++)
		{
			Debug.DrawLine(points[i], points[i + 1]);
		}
	}
}

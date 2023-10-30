using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.CameraSystem
{
	public class CameraOrtographics
	{
		private Camera main;

		public CameraOrtographics(Camera main)
		{
			this.main = main;
		}

		public void SetSize(float size)
		{
			main.orthographicSize = size;
		}
	}
}
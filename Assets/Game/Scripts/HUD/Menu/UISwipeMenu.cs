using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Game.HUD.Menu
{
	public class UISwipeMenu : MonoBehaviour
	{
		public Vector3 TopPoint
		{
			get
			{
				if(topPoint == Vector3.zero)
				{
					topPoint = sprites.Last().transform.position;
					topPoint.y += sprites.Last().bounds.size.y / 2;
				}

				return topPoint;
			}
		}
		private Vector3 topPoint;

		[SerializeField] private VerticalCamera verticalCamera;
		[SerializeField] private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
		[SerializeField] private bool isFitSpriteWidth = true;

		private void OnDrawGizmos()
		{
			for (int i = 0; i < sprites.Count; i++)
			{
				SpriteRenderer sprite = sprites[i];

				if (i == 0)
				{
					Vector3 bottom = verticalCamera.BottomPoint;
					bottom.y += sprite.bounds.size.y / 2;
					sprite.transform.position = bottom;
				}
				else
				{
					Vector3 bottom = sprites[i - 1].transform.position;
					bottom.y += sprites[i - 1].bounds.size.y / 2;//last top
					bottom.y += sprite.bounds.size.y / 2;
					sprite.transform.position = bottom;
				}

				if (isFitSpriteWidth)
				{
					sprite.transform.localScale = new Vector3(verticalCamera.FrustumWidth, sprite.transform.localScale.y);
				}
			}
		}
	}
}
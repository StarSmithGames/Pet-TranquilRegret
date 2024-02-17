using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Systems.InfinityRoadSystem
{
	public sealed class RoadBackground : MonoBehaviour
	{
		/// <summary>
		/// TODO: Optimize
		/// </summary>
		/// <returns></returns>
		public List<SpriteRenderer> GetSprites()
		{
			return  transform.GetComponentsInChildren<SpriteRenderer>().ToList();
		}
		
		public void Refresh( Vector3 startPoint )
		{
			var sprites = GetSprites();

			for (int i = 0; i < sprites.Count; i++)
			{
				SpriteRenderer sprite = sprites[i];

				if (i == 0)
				{
					startPoint.y += sprite.bounds.size.y / 2;
					startPoint.z = 0;
					sprite.transform.position = startPoint;
				}
				else
				{
					Vector3 bottom = sprites[i - 1].transform.position;
					bottom.y += sprites[i - 1].bounds.size.y / 2;//last top
					bottom.y += sprite.bounds.size.y / 2;
					bottom.z = 0;
					sprite.transform.position = bottom;
				}
			}
		}
	}
}
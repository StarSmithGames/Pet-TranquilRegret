using Sirenix.Utilities;

using System.Collections.Generic;

using UnityEngine;

namespace Game.Extensions
{
	public static class SpriteRendererExtensions
	{
		public static Sprite ConvertToSprite(this Texture2D texture)
		{
			return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
		}

		public static Bounds CalculateBounds(this IList<SpriteRenderer> renderers)
		{
			var bounds = new Bounds();
			renderers.ForEach(item => { bounds.Encapsulate(item.bounds); });

			return bounds;
		}

		public static Bounds CalculateBounds(this SpriteRenderer[] renderers)
		{
			var bounds = new Bounds();
			renderers.ForEach(item => { bounds.Encapsulate(item.bounds); });

			return bounds;
		}
	}
}
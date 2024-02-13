using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StarSmithGames.Core
{
    public static class FadeExtensions
    {
        public static void SetAlpha( this Image image, float alpha )
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }

        public static void SetAlpha( this SpriteRenderer spriteRenderer, float alpha )
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }

        public static void SetAlpha( this TextMeshPro text, float alpha )
        {
            Color color = text.color;
            color.a = alpha;
            text.color = color;
        }
    }
}
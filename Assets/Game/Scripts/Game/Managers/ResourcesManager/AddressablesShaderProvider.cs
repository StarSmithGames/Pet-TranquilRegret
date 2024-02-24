using UnityEngine;

namespace Game.Managers.AssetManager
{
	public static class AddressablesShaderProvider
	{
		public static void RefreshGameObject( GameObject gameObject )
		{
			var renderers = gameObject.GetComponentsInChildren< Renderer >();
                    
			foreach (var renderer in renderers)
			{
				foreach (var material in renderer.materials)
				{
					var shader = Shader.Find( material.shader.name );
					if ( shader == null )
					{
						Debug.LogError( $"[Asset] Shader {material.shader.name} = Null" );
						continue;
					}
					material.shader = shader;
				}
			}
		}

		public static void RefreshScene()
		{
			var renderers = GameObject.FindObjectsOfType< MeshRenderer >();
                    
			foreach (var meshRenderer in renderers)
			{
				foreach (var material in meshRenderer.materials)
				{
					var shader = Shader.Find( material.shader.name );
					if ( shader == null )
					{
						Debug.LogError( $"[Asset] Shader {material.shader.name} = Null" );
						continue;
					}
					material.shader = shader;
				}
			}
		}
	}
}
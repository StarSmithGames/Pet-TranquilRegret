using System.IO;

using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Systems.EnvironmentSystem
{
	[CreateAssetMenu(fileName = "CubemapConfig", menuName = "Game/CubemapConfig")]
	public class CubemapConfig : ScriptableObject
	{
		[HideInInspector] public Texture2D[] textures = new Texture2D[6];
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(CubemapConfig))]
	public class CubemapConfigEditor : Editor
	{
		private string[] labels = new string[]
		{
			"Right(+X)",	"Left(-X)",
			"Top(+Y)",		"Bottom(-Y)",
			"Front(+Z)",	"Back(-Z)"
		};

		private TextureFormat[] formatsHDR = new TextureFormat[]
		{
			TextureFormat.ASTC_HDR_10x10,
			TextureFormat.ASTC_HDR_12x12,
			TextureFormat.ASTC_HDR_4x4,
			TextureFormat.ASTC_HDR_5x5,
			TextureFormat.ASTC_HDR_6x6,
			TextureFormat.ASTC_HDR_8x8,
			TextureFormat.BC6H,
			TextureFormat.RGBAFloat,
			TextureFormat.RGBAHalf
		};

		private Vector2Int[] placementRects = new Vector2Int[]
		{
			new Vector2Int(2, 1),
			new Vector2Int(0, 1),
			new Vector2Int(1, 2),
			new Vector2Int(1, 0),
			new Vector2Int(1, 1),
			new Vector2Int(3, 1),
		};

		private CubemapConfig config;

		private void OnEnable()
		{
			config = target as CubemapConfig;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			for (int i = 0; i < 6; i++)
			{
				config.textures[i] = EditorGUILayout.ObjectField(labels[i], config.textures[i], typeof(Texture2D), false) as Texture2D;
			}

			if (GUILayout.Button("Build Cubemap"))
			{
				if (config.textures.Any((t) => t == null))
				{
					EditorUtility.DisplayDialog("[CubemapConfig] Cubemap Builder Error", "One or more texture is missing.", "Ok");
					return;
				}

				// Get size
				var size = config.textures[0].width;
				if (config.textures.Any((t) => (t.width != size) || (t.height != size)))
				{
					EditorUtility.DisplayDialog("[CubemapConfig] Cubemap Builder Error", "All the textures need to be the same size and square.", "Ok");
					return;
				}

				var isHDR = formatsHDR.Any((f) => f == config.textures[0].format);
				var texturePaths = config.textures.Select((t) => AssetDatabase.GetAssetPath(t)).ToArray();

				// Should be ok, ask for the file path.
				var path = EditorUtility.SaveFilePanel("Save Cubemap", Path.GetDirectoryName(texturePaths[0]), "Cubemap", isHDR ? "exr" : "png");

				if (string.IsNullOrEmpty(path)) return;

				// Save the readable flag to restore it afterwards
				var readableFlags = config.textures.Select(t => t.isReadable).ToArray();

				// Get the importer and mark the textures as readable
				var importers = texturePaths.Select((p) => TextureImporter.GetAtPath(p) as TextureImporter).ToArray();

				foreach (var importer in importers)
				{
					importer.isReadable = true;
				}

				AssetDatabase.Refresh();

				foreach (var p in texturePaths)
				{
					AssetDatabase.ImportAsset(p);
				}

				// Build the cubemap texture
				var cubeTexture = new Texture2D(size * 4, size * 3, isHDR ? TextureFormat.RGBAFloat : TextureFormat.RGBA32, false);

				for (int i = 0; i < 6; i++)
				{
					cubeTexture.SetPixels(placementRects[i].x * size, placementRects[i].y * size, size, size, config.textures[i].GetPixels(0));
				}

				cubeTexture.Apply(false);

				// Save the texture to the specified path, and destroy the temporary object
				var bytes = isHDR ? cubeTexture.EncodeToEXR() : cubeTexture.EncodeToPNG();

				File.WriteAllBytes(path, bytes);

				DestroyImmediate(cubeTexture);

				// Reset the read flags, and reimport everything
				for (var i = 0; i < 6; i++)
				{
					importers[i].isReadable = readableFlags[i];
				}

				path = path.Remove(0, Application.dataPath.Length - 6);

				AssetDatabase.ImportAsset(path);

				var cubeImporter = AssetImporter.GetAtPath(path) as TextureImporter;
				cubeImporter.textureShape = TextureImporterShape.TextureCube;
				cubeImporter.sRGBTexture = false;
				cubeImporter.generateCubemap = TextureImporterGenerateCubemap.FullCubemap;

				foreach (var p in texturePaths)
				{
					AssetDatabase.ImportAsset(p);
				}
				AssetDatabase.ImportAsset(path);

				AssetDatabase.Refresh();
			}
		}
	}
#endif
}
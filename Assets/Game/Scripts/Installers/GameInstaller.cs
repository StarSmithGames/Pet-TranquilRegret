using Game.Managers.CharacterManager;
using Game.Managers.GameManager;
using Game.Managers.LevelManager;
using Game.Managers.SceneManager;
using Game.Systems.CameraSystem;
using Game.Systems.NavigationSystem;
using Game.UI;

using UnityEditor;

using UnityEngine;

using Zenject;

namespace Game.Installers
{
	public class GameInstaller : MonoInstaller<GameInstaller>
	{
		public CameraSystem cameraSystem;
		public UISubCanvas subCanvas;
		public Joystick joystick;

		public override void InstallBindings()
		{
			Container.BindInstance(cameraSystem);
			Container.BindInstance(subCanvas);
			Container.BindInstance(joystick);

			CharacterManagerInstaller.Install(Container);

			var sceneManager = Container.Resolve<SceneManager>();
			var scenePath = sceneManager.ScenePathes[sceneManager.CurrentSceneNameImmediately];
			var assetPath = scenePath.Replace(".unity", ".asset");
			LevelSettings levelSettings = AssetDatabase.LoadAssetAtPath<LevelSettings>(assetPath);
			Debug.LogError((levelSettings != null));
		}
	}
}
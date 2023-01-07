using Game.Managers.AsyncManager;
using Game.Managers.CharacterManager;
using Game.Managers.SceneManager;

using System.Collections;

using UnityEngine;
using UnityEngine.Assertions;

using Zenject;

namespace Game.Systems.SpawnSystem
{
	public class SpawnSystem : IInitializable
	{
		private Player.Factory playerFactory;
		private CharacterManager characterManager;
		private SceneManager sceneManager;
		private AsyncManager asyncManager;

		public SpawnSystem(
			Player.Factory playerFactory,
			CharacterManager characterManager,
			SceneManager sceneManager,
			AsyncManager asyncManager)
		{
			this.playerFactory = playerFactory;
			this.characterManager = characterManager;
			this.sceneManager = sceneManager;
			this.asyncManager = asyncManager;
		}

		public void Initialize()
		{
			if (sceneManager.IsCurrentSceneMenu)
			{
				Debug.LogError("IS MENU");
			}
			else
			{
				asyncManager.StartCoroutine(SpawnPlayerWithDelay());

				Debug.LogError("IS DEBUG LEVEL");
			}
		}

		private void SpawnPlayer()
		{
			var points = GameObject.FindObjectsOfType<SpawnPoint>();

			Assert.IsTrue(points.Length > 0);

			var player = playerFactory.Create();
			var point = points.RandomItem();
			player.transform.position = point.transform.position;
			player.transform.rotation = point.transform.rotation;

			characterManager.SetPlayer(player);
		}

		private IEnumerator SpawnPlayerWithDelay()
		{
			yield return new WaitForSeconds(1f);

			SpawnPlayer();
		}
	}
}
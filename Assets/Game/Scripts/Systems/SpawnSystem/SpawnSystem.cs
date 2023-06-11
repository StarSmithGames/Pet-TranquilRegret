using Game.Entities;
using Game.Managers.CharacterManager;

using StarSmithGames.Go.AsyncManager;

using System.Collections;

using UnityEngine;
using UnityEngine.Assertions;

using Zenject;

namespace Game.Systems.SpawnSystem
{
	public class SpawnSystem : IInitializable
	{
		private Player.Factory playerFactory;
		private CameraSystem.CameraSystem cameraSystem;
		private CharacterManager characterManager;

		public SpawnSystem(
			Player.Factory playerFactory,
			CameraSystem.CameraSystem cameraSystem,
			CharacterManager characterManager)
		{
			this.playerFactory = playerFactory;
			this.cameraSystem = cameraSystem;
			this.characterManager = characterManager;
		}

		public void Initialize()
		{
			SpawnPlayer();
		}

		private void SpawnPlayer()
		{
			var points = GameObject.FindObjectsOfType<SpawnPoint>();

			Assert.IsTrue(points.Length > 0);

			var player = playerFactory.Create();
			var point = points.RandomItem();
			player.transform.position = point.GetRootPosition();
			player.Model.rotation = point.GetModelRotation();
			player.PlayerAvatar.CameraFollowPivot.position = point.GetFollowPosition();
			player.PlayerAvatar.CameraLookAtPivot.position = point.GetLookAtPosition();

			cameraSystem.SetTracketOffsetDirection(point.GetTracketObjectOffset());

			point.gameObject.SetActive(false);

			characterManager.SetPlayer(player);
		}

		private IEnumerator SpawnPlayerWithDelay()
		{
			yield return new WaitForSeconds(1f);

			SpawnPlayer();
		}

		//private void OnSceneChanged(SignalSceneChanged signal)
		//{
		//	SpawnPlayer();
		//}
	}
}
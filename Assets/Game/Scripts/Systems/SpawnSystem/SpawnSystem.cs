using Game.Entities;
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
		private SignalBus signalBus;
		private Player.Factory playerFactory;
		private CameraSystem.CameraSystem cameraSystem;
		private CharacterManager characterManager;
		private AsyncManager asyncManager;

		public SpawnSystem(
			SignalBus signalBus,
			Player.Factory playerFactory,
			CameraSystem.CameraSystem cameraSystem,
			CharacterManager characterManager,
			AsyncManager asyncManager)
		{
			this.signalBus = signalBus;
			this.playerFactory = playerFactory;
			this.cameraSystem = cameraSystem;
			this.characterManager = characterManager;
			this.asyncManager = asyncManager;
		}

		public void Initialize()
		{
			//signalBus?.Subscribe<SignalSceneChanged>(OnSceneChanged);

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

		private void OnSceneChanged(SignalSceneChanged signal)
		{
			SpawnPlayer();
		}
	}
}
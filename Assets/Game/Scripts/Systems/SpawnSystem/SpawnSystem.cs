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
		private CharacterManager characterManager;
		private AsyncManager asyncManager;

		public SpawnSystem(
			SignalBus signalBus,
			Player.Factory playerFactory,
			CharacterManager characterManager,
			AsyncManager asyncManager)
		{
			this.signalBus = signalBus;
			this.playerFactory = playerFactory;
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
			player.transform.position = point.transform.position;
			player.transform.rotation = point.transform.rotation;

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
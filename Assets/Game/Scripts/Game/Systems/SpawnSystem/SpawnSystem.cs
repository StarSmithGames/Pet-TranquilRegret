using StarSmithGames.Core;

using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Systems.SpawnSystem
{
	public class SpawnSystem
	{
		public void SpawnPlayer()
		{
			var points = GameObject.FindObjectsOfType<SpawnPoint>();

			Assert.IsTrue(points.Length > 0);

			var point = points.RandomItem();
			point.Spawn();
			point.gameObject.SetActive(false);
			
		    Debug.LogError( "Spawned" );
		}
	}
}
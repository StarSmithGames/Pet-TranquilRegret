using Game.Managers.LevelManager;
using Game.Systems.FloatingSystem;
using UnityEngine;
using Zenject;

namespace Game.Entities
{
    public class Coin : Floating3DObject
	{
		[Min(1)]
		[SerializeField] private int count = 1;

		private LevelManager levelManager;
		private FloatingSystem floatingSystem;

		[Inject]
		private void Construct(LevelManager levelManager, FloatingSystem floatingSystem)
		{
			this.levelManager = levelManager;
			this.floatingSystem = floatingSystem;
		}

		protected override void OnAnimationCompleted()
		{
			floatingSystem.CreateText(target.position, $"+{count}");

			levelManager.CurrentLevel.Coins.CurrentValue += count;
		}
	}
}
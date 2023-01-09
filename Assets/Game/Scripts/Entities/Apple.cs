using Game.Managers.LevelManager;
using Game.Systems.FloatingSystem;
using UnityEngine;

using Zenject;

namespace Game.Entities
{
	public class Apple : Floating3DObject
	{
		[SerializeField] private CountableGoalData data;
		[Min(1)]
		[SerializeField] private int count;

		private FloatingSystem floatingSystem;

		[Inject]
		private void Construct(FloatingSystem floatingSystem)
		{
			this.floatingSystem = floatingSystem;
		}

		protected override void OnAnimationCompleted()
		{
			floatingSystem.CreateText(target.position, $"+{count}", data.information.portrait);
		}
	}
}
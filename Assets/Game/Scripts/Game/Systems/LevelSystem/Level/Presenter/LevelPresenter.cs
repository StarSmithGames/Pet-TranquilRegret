using Game.Managers.PauseManager;
using System;
using UnityEngine;

namespace Game.Systems.LevelSystem
{
	public sealed class LevelPresenter : IDisposable, IPausable
	{
		public LevelModel Model { get; }
		public LevelGameplay Gameplay { get; }
		public EstimatedTimer Timer { get; }

		public LevelPresenter(
			LevelModel model,
			LevelGameplay gameplay,
			EstimatedTimer timer
			)
		{
			Model = model ?? throw new ArgumentNullException( nameof(model) );
			Gameplay = gameplay ?? throw new ArgumentNullException( nameof(gameplay) );
			Timer = timer ?? throw new ArgumentNullException( nameof(timer) );
		}

		public void Dispose()
		{
			Timer.Stop();
		}

		public void Start()
		{
			Timer.StartEstimatedTimer( Model.Config.estimatedTime );
		}

		public float GetProgress01()
		{
			var goals = Gameplay.GoalRegistrator.GoalsPrimary;
			float percents = 0;
			for (int i = 0; i < goals.Count; i++)
			{
				percents += goals[i].PercentValue;
			}

			return percents / goals.Count;
		}

		public float GetProgress()
		{
			return Mathf.Round(GetProgress01() * 100f);
		}

		public void Complete()
		{
			Timer.Stop();
		}

		public void Lose()
		{
			Dispose();
			//gameLoader.LoadMenu();
		}

		public void Leave()
		{
			Dispose();
			//gameLoader.LoadMenu();
		}

		public void Pause()
		{
			Timer.Pause();
		}

		public void UnPause()
		{
			Timer.UnPause();
		}
	}
}
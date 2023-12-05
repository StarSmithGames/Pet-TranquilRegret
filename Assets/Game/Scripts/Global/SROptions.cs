#if !DISABLE_SRDEBUGGER
using Game.Systems.LevelSystem;
using Game.Systems.StorageSystem;

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using UnityEngine;

using Zenject;

public partial class SROptions
{
	[Inject] private StorageSystem storageSystem;
	[Inject] private SignalBus signalBus;

	public SROptions()
	{
		SRDebug.Instance.PanelVisibilityChanged += OnPanelVisibilityChanged;
	}

	#region Level
	[Category("Level")]
	[Sort(4)]
	public void CompleteLevel()
	{
		var level = storageSystem.IntermediateData.LevelPresenter;
		if (level == null) return;

		level.Complete();
	}

	[Category("Level")]
	[Sort(4)]
	public void CompleteLevelLastStep()
	{
		//App.Instance.Dispatch(new SignalLevelCompleteLastMoveCheat());
	}

	[NumberRange(1, 1000)]
	[Category("Level")]
	public int FastTravel
	{
		get => fastTravel;
		set
		{
			fastTravel = Mathf.Clamp(value, 1, storageSystem.IntermediateData.GameplayConfig.levels.Count);

			OnFastTraveled();
		}
	}
	private int fastTravel = 1;

	private void OnFastTraveled()
	{
		//var data = storageSystem.GamePlayData.Storage.GameProgress.GetData();
		//data.progressMainIndex = fastTravel - 1;
		storageSystem.Save();

		RefreshLevel();
	}

	private void RefreshLevel()
	{
		//fastTravel = storageSystem.GamePlayData.Storage.GameProgress.GetData().progressMainIndex + 1;

		signalBus?.Fire(new SignalOnLevelChangedCheat());
	}
	#endregion

	private void OnPanelVisibilityChanged(bool isVisible)
	{
		if (!isVisible)
			return;

		ProjectContext.Instance.Container.Inject(this);

		//_gamePlayDataService.IntermediateData.OnLevelChanged += LevelChanged;
		//_gamePlayDataService.StorageData.Lifes.onChanged += LifesChanged;

		RefreshLevel();
	}
}
#endif
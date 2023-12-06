using DG.Tweening;

using Game.Services;
using Game.Systems.LevelSystem;

using StarSmithGames.Core;
using StarSmithGames.Go;

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.UI
{
	public class LevelFinishDialog : ViewBase
	{
		public Transform shineEffect;
		public Transform ribbon;
		public List<Transform> stars = new();
		public GameObject rewardsRoot;
		public Transform rewardsContent;
		public Button buttonExit;

		[Inject] private ViewService viewService;
		[Inject] private LevelManager levelManager;

		private void Awake()
		{
			viewService.ViewDialogRegistrator.Registrate(this);
		}

		private void OnDestroy()
		{
			viewService.ViewDialogRegistrator.UnRegistrate(this);
		}

		public override void Show(Action callback = null)
		{
			IsInProcess = true;
			canvasGroup.alpha = 0f;
			canvasGroup.Enable(true, false);
			IsShowing = true;

			Interactable(true);

			ribbon.localScale = Vector3.zero;
			shineEffect.localScale = Vector3.zero;
			for (int i = 0; i < stars.Count; i++)
			{
				stars[i].transform.localScale = Vector3.zero;
			}

			rewardsRoot.SetActive(false);

			var starsSequence = DOTween.Sequence();
			for (int i = 0; i < stars.Count; i++)
			{
				if (i + 1 <= 3)
				{
					var item = stars[i];
					starsSequence
						.AppendInterval(0.16f)
						.AppendCallback(() =>
						{
							item.DOScale(1f, 0.16f);
						});
				}
			}

			sequence?.Kill(true);
			sequence = DOTween.Sequence();
			sequence
				.Append(canvasGroup.DOFade(1f, 0.2f))
				.Append(ribbon.DOScale(1f, 0.33f).SetEase(Ease.OutBounce))
				.Join(shineEffect.DOScale(1f, 0.16f).SetEase(Ease.OutQuart))
				.Append(starsSequence)
				.AppendCallback(() =>
				{
					callback?.Invoke();
					IsInProcess = false;
				});
		}

		private void Interactable(bool trigger)
		{
			buttonExit.interactable = trigger;
		}

		public void OnExitClicked()
		{
			Interactable(false);

			levelManager.LeaveLevel();
		}
	}
}
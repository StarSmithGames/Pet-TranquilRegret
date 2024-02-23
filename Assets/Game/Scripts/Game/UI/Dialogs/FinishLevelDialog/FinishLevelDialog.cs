using DG.Tweening;
using Game.Systems.GameSystem;
using Game.Systems.LevelSystem;

using StarSmithGames.Core;

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.UI
{
	public sealed class FinishLevelDialog : UIViewDialog
	{
		public Transform shineEffect;
		public Transform ribbon;
		public List<Transform> stars = new();
		public GameObject rewardsRoot;
		public Transform rewardsContent;
		public Button buttonExit;
		[Space]
		public UIAward AwardPrefab;

		private List<Transform> awardsTransforms = new();

		private GameService _gameService;
		private LevelManager _levelManager;
		
		[Inject]
		private void Construct(
			GameService gameService,
			LevelManager levelManager
			)
		{
			_gameService = gameService;
			_levelManager = levelManager;
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

			var awardsSequence = DOTween.Sequence();

			var awards = _levelManager.CurrentLevel.Presenter.Model.Config.awards;
			rewardsRoot.SetActive(awards.Count > 0);
			rewardsContent.DestroyChildren();

			for (int i = 0; i < awards.Count; i++)
			{
				var award = GameObject.Instantiate( AwardPrefab, rewardsContent );
				award.transform.localScale = Vector3.zero;
				award.SetAward(awards[i]);

				awardsTransforms.Add(award.transform);
			}

			for (int i = 0; i < awardsTransforms.Count; i++)
			{
				var award = awardsTransforms[i];
				awardsSequence
					.AppendInterval(0.16f)
					.AppendCallback(() =>
					{
						award.DOScale(1f, 0.16f);
					});
			}

			sequence?.Kill(true);
			sequence = DOTween.Sequence();
			sequence
				.Append(canvasGroup.DOFade(1f, 0.2f))
				.Append(ribbon.DOScale(1f, 0.33f).SetEase(Ease.OutBounce))
				.Join(shineEffect.DOScale(1f, 0.16f).SetEase(Ease.OutQuart))
				.Append(starsSequence)
				.Append(awardsSequence)
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

			_gameService.LeaveLevel();
		}
	}
}
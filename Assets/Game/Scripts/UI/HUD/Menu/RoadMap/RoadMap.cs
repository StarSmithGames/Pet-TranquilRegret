using DG.Tweening;

using Game.Systems.GameSystem;
using Game.UI;
using Game.VFX;

using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;
using UnityEngine.UIElements;

using Zenject;

namespace Game.HUD.Menu
{
	public partial class RoadMap : MonoBehaviour
	{
		public bool IsInProcess { get; private set; }

		public Vector3 TopPoint
		{
			get
			{
				if(topPoint == Vector3.zero)
				{
					topPoint = sprites.Last().transform.position;
					topPoint.y += sprites.Last().bounds.size.y / 2;
				}

				return topPoint;
			}
		}
		private Vector3 topPoint;

		[SerializeField] private VerticalCamera verticalCamera;
		[Header("Background")]
		[SerializeField] private Transform backgroundsContent;
		[SerializeField] private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
		[SerializeField] private bool isFitSpriteWidth = true;
		[Header("Levels")]
		[SerializeField] private Transform levelsContent;
		[SerializeField] private List<UIRoadLevel> levels = new List<UIRoadLevel>();
		[SerializeField] private List<LevelConnection> connections = new List<LevelConnection>();

		private UIRoadPin Pin => menuCanvas.Pin;

		private int lastIndex = -1;
		private GameProgress gameProgress;

		private GameData gameData;
		private UIMenuCanvas menuCanvas;
		private ParticalVFXFootStep.Factory pawStepFactory;

		[Inject]
		private void Construct(
			GameData gameData,
			UIMenuCanvas menuCanvas,
			[Inject(Id = "StepPawVerticalPrint")] ParticalVFXFootStep.Factory pawStepFactory)
		{
			this.gameData = gameData;
			this.menuCanvas = menuCanvas;
			this.pawStepFactory = pawStepFactory;
		}

		private void Start()
		{
			BackgroundFullRefresh();

			if (gameData.IsFirstTime)
			{
				gameData.IsFirstTime = false;
				gameData.StorageKeeper.GetStorage().GameProgress.SetData(new GameProgress()
				{
					progressMainIndex = 0,
				});
				gameProgress = gameData.StorageKeeper.GetStorage().GameProgress.GetData();
				lastIndex = gameProgress.progressMainIndex;
				gameData.StorageKeeper.Save();

				OpenRoadMap(true);
			}
			else
			{
				gameProgress = gameData.StorageKeeper.GetStorage().GameProgress.GetData();
				lastIndex = gameProgress.progressMainIndex;

				OpenRoadMap(false);
			}

			LevelsRefresh();
		}

		private void BackgroundFullRefresh()
		{
			for (int i = 0; i < sprites.Count; i++)
			{
				SpriteRenderer sprite = sprites[i];

				if (i == 0)
				{
					Vector3 bottom = verticalCamera.BottomPoint;
					bottom.y += sprite.bounds.size.y / 2;
					sprite.transform.position = bottom;
				}
				else
				{
					Vector3 bottom = sprites[i - 1].transform.position;
					bottom.y += sprites[i - 1].bounds.size.y / 2;//last top
					bottom.y += sprite.bounds.size.y / 2;
					sprite.transform.position = bottom;
				}

				if (isFitSpriteWidth)
				{
					sprite.transform.localScale = new Vector3(verticalCamera.FrustumWidth, sprite.transform.localScale.y);
				}
			}
		}

		private void LevelsRefresh()
		{
			var config = gameData.GameplayConfig;
			Assert.AreEqual(config.levels.Count, levels.Count);

			for (int i = 0; i < levels.Count; i++)
			{
				levels[i]
					.SetLevel(config.levels[i])
					.Enable(i <= gameProgress.progressMainIndex)
					.onClicked += OnLevelClicked;
			}
		}

		private void OpenRoadMap(bool isFirstTime)
		{
			if (isFirstTime)
			{
				StartCoroutine(FirstTime());
			}
			else
			{
				Vector3 position = levels[gameProgress.progressMainIndex].transform.position;
				verticalCamera.SetPosition(position);
				Pin.transform.position = position;
			}
		}

		private IEnumerator FirstTime()
		{
			IsInProcess = true;

			Pin.transform.position = new Vector3(0, -100, 0);

			yield return new WaitForSeconds(0.5f);

			var points = connections.First().GetPoints();

			Pin.transform
				.DOPath(points, 1.5f)
				.OnUpdate(() =>
				{
					Step(pawStepFactory);
				})
				.OnComplete(() => IsInProcess = false);
		}


		private void ShowLevelWindow()
		{
			var window = menuCanvas.ViewRegistrator.GetAs<LevelWindow>();
			window.SetLevel(gameData.GetLevelConfig(lastIndex));
			window.Show();
		}

		private void OnLevelClicked(UIRoadLevel level)
		{
			if (IsInProcess) return;

			Pin.transform.DOKill(true);

			if (level.IsEnable)
			{
				int index = levels.IndexOf(level);
				int diff = index - lastIndex;
				lastIndex = index;

				if (diff != 0)
				{
					IsInProcess = true;

					if (diff == 1)//up on 1
					{
						var connection = connections[levels.IndexOf(level)];
						var points = connection.GetPoints();

						Sequence sequence = DOTween.Sequence();

						sequence
							.Append(Pin.transform.DOPath(points, 1.5f)
								.OnUpdate(() =>
								{
									Step(pawStepFactory);
								}))
							.OnComplete(() =>
							{
								IsInProcess = false;

								ShowLevelWindow();
							});
					}
					else//down or up
					{
						Sequence sequence = DOTween.Sequence();

						sequence
							.Append(Pin.transform.DOScale(0, 0.2f))
							.AppendCallback(() => Pin.transform.position = level.transform.position)
							.Append(Pin.transform.DOScale(1, 0.25f).SetEase(Ease.OutBounce))
							.OnComplete(() =>
							{
								IsInProcess = false;

								ShowLevelWindow();
							});
					}
				}
				else
				{
					ShowLevelWindow();
				}
			}
			//else
			//{
			//	//disabled
			//}
		}

		[Button(DirtyOnClick = true)]
		private void Fill()
		{
			sprites = backgroundsContent.GetComponentsInChildren<SpriteRenderer>().ToList();
			levels = levelsContent.GetComponentsInChildren<UIRoadLevel>().ToList();
			connections = levelsContent.GetComponentsInChildren<LevelConnection>().ToList();
		}

		private void OnDrawGizmos()
		{
			BackgroundFullRefresh();
		}
	}

	/// <summary>
	/// Paw steps
	/// </summary>
	public partial class RoadMap
	{
		[Space]
		[SerializeField] private float stepDelta = 0.7f;
		[SerializeField] private float stepGap = 0.5f;

		private Vector3 lastStepEmit;
		private int stepDir = 1;

		private void Step(ParticalVFXFootStep.Factory factory)
		{
			if (Vector3.Distance(lastStepEmit, menuCanvas.Pin.transform.position) > stepDelta)
			{
				var step = factory.Create();
				stepDir *= -1;

				Vector3 toPosition = (menuCanvas.Pin.transform.position - lastStepEmit).normalized;
				float angleToPosition = Vector3.Angle(Vector3.up, toPosition);

				step.Play(new ParticleSystem.EmitParams()
				{
					position = menuCanvas.Pin.transform.position + (menuCanvas.Pin.transform.right * stepGap * stepDir),
					rotation = angleToPosition,
				});

				lastStepEmit = menuCanvas.Pin.transform.position;
			}
		}
	}
}
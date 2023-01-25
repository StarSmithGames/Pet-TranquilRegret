using DG.Tweening;

using Game.Managers.LevelManager;
using Game.Managers.SceneManager;
using Game.Managers.StorageManager;
using Game.UI;
using Game.VFX;

using ModestTree;

using Sirenix.OdinInspector;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

using Zenject;

namespace Game.HUD.Menu
{
	public partial class RoadMap : MonoBehaviour
	{
		public bool IsInTransition { get; private set; }

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
		[Space]
		[SerializeField] private Transform backgroundsContent;
		[SerializeField] private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
		[SerializeField] private bool isFitSpriteWidth = true;
		[Space]
		[SerializeField] private Transform levelsContent;
		[SerializeField] private List<UIRoadLevel> levels = new List<UIRoadLevel>();
		[SerializeField] private List<LevelConnection> connections = new List<LevelConnection>();

		private Data data;
		private UIRoadPin Pin => menuCanvas.Pin;

		private UIMenuCanvas menuCanvas;
		private ISaveLoad saveLoad;
		private SceneManager sceneManager;
		private ParticalVFXFootStep.Factory pawStepFactory;

		[Inject]
		private void Construct(
			UIMenuCanvas menuCanvas,
			ISaveLoad saveLoad,
			SceneManager sceneManager,
			[Inject(Id = "StepPawVerticalPrint")] ParticalVFXFootStep.Factory pawStepFactory)
		{
			this.menuCanvas = menuCanvas;
			this.saveLoad = saveLoad;
			this.sceneManager = sceneManager;
			this.pawStepFactory = pawStepFactory;
		}

		private void Start()
		{
			BackgroundFullRefresh();

			data = saveLoad.GetStorage().Profile.GetData().map;

			for (int i = 0; i < levels.Count; i++)
			{
				if (i < data.levels.Count)
				{
					levels[i]
						.SetLevel(data.levels[i])
						.Enable(true)
						.onClicked += OnLevelClicked;
				}
				else
				{
					levels[i]
						.SetLevel(null)
						.Enable(false)
						.onClicked += OnLevelClicked;
				}
			}

			if (saveLoad.GetStorage().IsFirstTime.GetData())
			{
				var level = new Level.Data();
				levels
					.First()
					.SetLevel(level)
					.Enable(true);
				data.levels.Add(level);

				StartCoroutine(FirstTime());
			}
			else
			{
				Vector3 position = levels[data.lastIndex].transform.position;
				verticalCamera.SetPosition(position);
				Pin.transform.position = position;
			}
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

		private IEnumerator FirstTime()
		{
			IsInTransition = true;

			Pin.transform.position = new Vector3(0, -100, 0);

			data.lastIndex = 0;

			yield return new WaitForSeconds(0.5f);

			var points = connections.First().GetPoints();

			Pin.transform
				.DOPath(points, 1.5f)
				.OnUpdate(() =>
				{
					Step(pawStepFactory);
				})
				.OnComplete(() => IsInTransition = false);
		}

		private void OnLevelClicked(UIRoadLevel level)
		{
			if (IsInTransition) return;

			Pin.transform.DOKill(true);

			if (level.IsEnable)
			{
				int index = levels.IndexOf(level);
				int diff = index - data.lastIndex;

				data.lastIndex = index;

				if (diff != 0)
				{
					IsInTransition = true;

					data.lastIndex = index;

					if (diff == 1)//up on 1
					{
						var connection = connections[levels.IndexOf(level)];
						var points = connection.GetPoints();

						var data = level.Level;

						Sequence sequence = DOTween.Sequence();

						sequence
							.Append(Pin.transform.DOPath(points, 1.5f)
								.OnUpdate(() =>
								{
									Step(pawStepFactory);
								}))
							.OnComplete(() =>
							{
								IsInTransition = false;

								if (data.stars > 0)
								{
									//Restart
								}
								else
								{
									//Start
								}

								ShowLevelWindow(index + 1);
							});
					}
					else//down
					{
						Sequence sequence = DOTween.Sequence();

						sequence
							.Append(Pin.transform.DOScale(0, 0.2f))
							.AppendCallback(() => Pin.transform.position = level.transform.position)
							.Append(Pin.transform.DOScale(1, 0.25f).SetEase(Ease.OutBounce))
							.OnComplete(() =>
							{
								IsInTransition = false;

								ShowLevelWindow(index + 1);
							});
					}
				}
				else
				{
					ShowLevelWindow(index + 1);
				}
			}
			else
			{
				//disabled
			}
		}

		private void ShowLevelWindow(int index)
		{
			sceneManager.GetLevelSettings(index, (settings) =>
			{
				if (settings != null)
				{
					var window = menuCanvas.WindowsRegistrator.GetAs<LevelWindow>();
					window.SetLevel(settings);
					window.Show();
				}
				else
				{
					Debug.LogError("LevelSettings == NULL");
				}
			});
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


		[System.Serializable]
		public class Data
		{
			public int lastIndex;
			public List<Level.Data> levels = new List<Level.Data>();
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
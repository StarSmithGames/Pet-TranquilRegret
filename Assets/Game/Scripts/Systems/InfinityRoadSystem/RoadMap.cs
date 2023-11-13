using DG.Tweening;

using Game.Entities;
using Game.HUD.Menu;
using Game.Installers;
using Game.Systems.CameraSystem;
using Game.Systems.GameSystem;
using Game.Systems.LevelSystem;
using Game.Systems.StorageSystem;
using Game.UI;
using Game.VFX;

using Sirenix.OdinInspector;

using StarSmithGames.Core;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

using Zenject;

namespace Game.Systems.InfinityRoadSystem
{
	public partial class RoadMap : MonoBehaviour
	{
		public bool IsInProcess { get; private set; }

		[Header("Background")]
		public Transform backgroundsContent;
		[Header("Levels")]
		public Transform levelsContent;
		public List<UIRoadLevel> levels = new();
		public List<LevelConnection> connections = new();
		public CloudsSettings cloudsSettings;

		[Inject] private VerticalCamera verticalCamera;
#if !DISABLE_SRDEBUGGER
		[Inject] private SignalBus signalBus;
#endif

		private UIRoadPin Pin => menuCanvas.Pin;

		private int lastIndex = -1;
		private GameProgress gameProgress;

		private GameData gameData;
		private UIMenuCanvas menuCanvas;
		private ParticalVFXFootStep.Factory pawStepFactory;

		private List<SpriteRenderer> sprites = new();

		[Inject]
		private void Construct(
			GameData gameData,
			UICanvas menuCanvas,
			[Inject(Id = "StepPawVerticalPrint")] ParticalVFXFootStep.Factory pawStepFactory)
		{
			this.gameData = gameData;
			this.menuCanvas = menuCanvas as UIMenuCanvas;
			this.pawStepFactory = pawStepFactory;
		}

		private void Awake()
		{
			gameProgress = gameData.Storage.GameProgress.GetData();
			lastIndex = gameProgress.progressMainIndex;

			cloudsSettings.clouds.SetLerp(0);
			BackgroundRefresh();
			OpenRoadMap(gameData.IsFirstTime);
			RefreshLevels();
			AssignLevels();
		}

		private void Start()
		{
#if !DISABLE_SRDEBUGGER
			signalBus?.Subscribe<SignalOnLevelChangedCheat>(OnLevelChangedCheat);
#endif
		}

		private void Update()
		{
			Vector2 pos = verticalCamera.transform.position;
			float lerp = Mathf.InverseLerp(cloudsSettings.GetWorldStartPoint().y, cloudsSettings.GetWorldEndPoint().y, pos.y);
			cloudsSettings.clouds.SetLerp(lerp);
		}

		public List<SpriteRenderer> GetSprites()
		{
			sprites = backgroundsContent.GetComponentsInChildren<SpriteRenderer>().ToList();

			return sprites;
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

			IEnumerator FirstTime()
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
		}

		private void RefreshLevels()
		{
			for (int i = 0; i < levels.Count; i++)
			{
				levels[i].Enable(i <= gameProgress.progressMainIndex);

			}
		}

		private void AssignLevels()
		{
			for (int i = 0; i < levels.Count; i++)
			{
				levels[i].onClicked += OnLevelClicked;
			}
		}

		private void BackgroundRefresh()
		{
			GetSprites();

			for (int i = 0; i < sprites.Count; i++)
			{
				SpriteRenderer sprite = sprites[i];

				if (i == 0)
				{
					Vector3 bottom = verticalCamera.BottomPoint;
					bottom.y += sprite.bounds.size.y / 2;
					bottom.z = 0;
					sprite.transform.position = bottom;
				}
				else
				{
					Vector3 bottom = sprites[i - 1].transform.position;
					bottom.y += sprites[i - 1].bounds.size.y / 2;//last top
					bottom.y += sprite.bounds.size.y / 2;
					bottom.z = 0;
					sprite.transform.position = bottom;
				}
			}
		}

		private void ShowLevelWindow()
		{
			var window = menuCanvas.ViewRegistrator.GetAs<LevelDialog>();
			window.SetLevel(gameData.IntermediateData.GetLevelConfig(lastIndex + 1));
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

#if !DISABLE_SRDEBUGGER
		private void OnLevelChangedCheat(SignalOnLevelChangedCheat signal)
		{
			RefreshLevels();
			OpenRoadMap(false);
		}
#endif

#if UNITY_EDITOR
		[Button(DirtyOnClick = true)]
		private void Refresh()
		{
			levels = levelsContent.GetComponentsInChildren<UIRoadLevel>().ToList();
			connections = levelsContent.GetComponentsInChildren<LevelConnection>().ToList();
		}
#endif

		private void OnDrawGizmosSelected()
		{
			if(verticalCamera == null)
			{
				verticalCamera = FindAnyObjectByType<MenuInstaller>().verticalCamera;
			}

			if (cloudsSettings.isFromTopCamera)
			{
				cloudsSettings.cloudsEndPoint = verticalCamera.TopPointCamera;
				EditorUtility.SetDirty(gameObject);
			}

			Gizmos.color = Color.green;
			Gizmos.DrawSphere(cloudsSettings.GetWorldStartPoint(), 5f);
			Gizmos.DrawSphere(cloudsSettings.GetWorldEndPoint(), 5f);


			BackgroundRefresh();
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

	[System.Serializable]
	public class CloudsSettings
	{
		public RoadClouds clouds;
		public Vector2 cloudsStartPoint;
		public Vector2 cloudsEndPoint;
		public bool isFromTopCamera = true;
		public bool isFromEndPoint = true;

		public Vector2 GetWorldStartPoint()
		{
			if (isFromEndPoint)
			{
				return cloudsEndPoint - cloudsStartPoint;
			}

			return cloudsStartPoint;
		}

		public Vector2 GetWorldEndPoint()
		{
			return cloudsEndPoint;
		}
	}
}
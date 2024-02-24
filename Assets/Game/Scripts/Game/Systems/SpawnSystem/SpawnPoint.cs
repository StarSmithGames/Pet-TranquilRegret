using Sirenix.OdinInspector;

using UnityEngine;

using StarSmithGames.Core;

using Game.Character;

using Zenject;

using Game.Managers.CharacterManager;
using Game.Systems.StorageSystem;
using Game.Systems.GameSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Systems.SpawnSystem
{
	[ExecuteInEditMode]
	[AddComponentMenu("AGame/Spawn/Spawn Point")]
	public class SpawnPoint : MonoBehaviour
	{
		public Vector3 FollowPosition => transform.position + settings.followPoint;
		public Vector3 LookPosition => transform.position + settings.lookAtPoint;
		public Vector3 TracketObjectOffset => settings.tracketObjectOffset * settings.cameraDistance;

		public Settings settings;

		[Inject] private DiContainer diContainer;
		[Inject] private CharacterManager characterManager;
		[Inject] private CameraSystem.CameraSystem cameraSystem;
		[Inject] private GameplayConfig gameplayConfig;

		public void Spawn()
		{
			Debug.LogError( gameplayConfig.CharacterReference.Asset != null );

			if ( !gameplayConfig.CharacterReference.IsLoaded )
			{
				gameplayConfig.CharacterReference.Load( () =>
				{
					Spawn( diContainer.InstantiatePrefab( gameplayConfig.CharacterReference.Asset ).GetComponent< Character.Character >() );
				} );
			}
			else
			{
				Spawn( diContainer.InstantiatePrefab( gameplayConfig.CharacterReference.Asset ).GetComponent< Character.Character >() );
			}
		}

		private void Spawn( Character.Character character )
		{
			character.Presenter.View.root.position = transform.position;
			character.Presenter.View.model.rotation = transform.rotation;

			character.Presenter.View.cameraFollowPivot.position = FollowPosition;
			character.Presenter.View.cameraLookAtPivot.position = LookPosition;

			if (settings.isPlayer)
			{
				characterManager.RegistratePlayer(character);
			}
			else
			{
				characterManager.Registrator.Registrate(character);
			}
			cameraSystem
				.SetTarget(character)
				.SetTracketOffsetDirection(TracketObjectOffset);
		}

#if UNITY_EDITOR
		[ContextMenu("Update Settings")]
		private void UpdateSettings()
		{
			var camera = FindAnyObjectByType<CameraSystem.CameraSystem>();

			if (!settings.isCustom)
			{
				if (settings.spawnPlace == SpawnPlace.Forward)
				{
					settings.followPoint = new Vector3(0, 4, 2);
					settings.lookAtPoint = settings.followPoint;
					settings.tracketObjectOffset = new Vector3(0, 1, -1);

					transform.rotation = Quaternion.Euler(0, 0, 0);
				}
				else if (settings.spawnPlace == SpawnPlace.Backward)
				{
					settings.followPoint = new Vector3(0, 4, -2);
					settings.lookAtPoint = settings.followPoint;
					settings.tracketObjectOffset = new Vector3(0, 1, 1);

					transform.rotation = Quaternion.Euler(0, 180, 0);
				}
				else if (settings.spawnPlace == SpawnPlace.Left)
				{
					settings.followPoint = new Vector3(-2, 4, 0);
					settings.lookAtPoint = settings.followPoint;
					settings.tracketObjectOffset = new Vector3(1, 1, 0);

					transform.rotation = Quaternion.Euler(0, -90, 0);
				}
				else if (settings.spawnPlace == SpawnPlace.Right)
				{
					settings.followPoint = new Vector3(2, 4, 0);
					settings.lookAtPoint = settings.followPoint;
					settings.tracketObjectOffset = new Vector3(-1, 1, 0);

					transform.rotation = Quaternion.Euler(0, 90, 0);
				}

				EditorUtility.SetDirty(this);
			}

			if (camera != null)
			{
				camera.folowPivotEditor.position = FollowPosition;
				camera.lookAtPivotEditor.position = LookPosition;
				camera.SetTracketOffsetDirection(TracketObjectOffset);
				EditorUtility.SetDirty(camera);
			}
		}

		private void Update()
		{
			if (Application.isPlaying) return;

			UpdateSettings();
		}

		private void OnValidate()
		{
			if (Application.isPlaying) return;

			UpdateSettings();
		}

		private void OnDrawGizmos()
		{
			var position = transform.position;

			position.y = 0f;
			if (Physics.Raycast(position + Vector3.up * 2f, Vector3.down, out var hit))
			{
				position.y = hit.point.y;
				Gizmos.color = Color.white;
				Gizmos.DrawLine(position + Vector3.up * 2f, position + Vector3.down * hit.point.y);
			}

			Gizmos.color = Color.blue;
			DrawArrow.ForGizmo(position, transform.forward);
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(position, 0.15f);

			Gizmos.color = Color.red;
			Gizmos.DrawSphere(FollowPosition, 0.15f);

			Gizmos.color = Color.gray;
			Gizmos.DrawSphere(LookPosition, 0.1f);

			Gizmos.color = Color.blue;
			DrawArrow.ForGizmo(FollowPosition, settings.tracketObjectOffset);
			DrawArrow.ForGizmo(LookPosition, settings.tracketObjectOffset);


			transform.position = position;
		}
#endif

		[System.Serializable]
		public class Settings
		{
			public float cameraDistance = 15f;

			public bool isCustom = false;
			[ShowIf("isCustom")]
			public Vector3 followPoint;
			[ShowIf("isCustom")]
			public Vector3 lookAtPoint;

			[ShowIf("isCustom")]
			public Vector3 tracketObjectOffset;
			
			[HideIf("isCustom")]
			public SpawnPlace spawnPlace = SpawnPlace.Forward;

			public bool isPlayer = true;
		}
	}

	public enum SpawnPlace
	{
		Left,
		Right,
		Forward,
		Backward,
	}
}
using Sirenix.OdinInspector;

using UnityEngine;

using StarSmithGames.Core;

using Game.Character;
using Game.Systems.GameSystem;

using Zenject;

using Game.Managers.CharacterManager;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Systems.SpawnSystem
{
	[ExecuteInEditMode]
	public class SpawnPoint : MonoBehaviour
	{
		public Vector3 FollowPosition => transform.position + settings.followPoint;
		public Vector3 LookPosition => transform.position + settings.lookAtPoint;
		public Vector3 TracketObjectOffset => settings.tracketObjectOffset * settings.cameraDistance;

		public Settings settings;

		[Inject] private DiContainer diContainer;
		[Inject] private GameData gameData;
		[Inject] private CharacterManager characterManager;
		[Inject] private CameraSystem.CameraSystem cameraSystem;

		public void Spawn()
		{
			var character = diContainer.InstantiatePrefab(gameData.GameplayConfig.characterPrefab).GetComponent<AbstractCharacter>();

			character.root.position = transform.position;
			character.model.rotation = transform.rotation;

			character.facade.cameraFollowPivot.position = FollowPosition;
			character.facade.cameraLookAtPivot.position = LookPosition;

			characterManager.Registrate(character);
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
			Gizmos.color = Color.blue;
			DrawArrow.ForGizmo(transform.position, transform.forward);
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(transform.position, 0.15f);

			Gizmos.color = Color.red;
			Gizmos.DrawSphere(FollowPosition, 0.15f);

			Gizmos.color = Color.gray;
			Gizmos.DrawSphere(LookPosition, 0.1f);

			Gizmos.color = Color.blue;
			DrawArrow.ForGizmo(FollowPosition, settings.tracketObjectOffset);
			DrawArrow.ForGizmo(LookPosition, settings.tracketObjectOffset);
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

			public SpawnType spawnType;
		}
	}

	public enum SpawnPlace
	{
		Left,
		Right,
		Forward,
		Backward,
	}

	public enum SpawnType
	{
		Player,
	}
}
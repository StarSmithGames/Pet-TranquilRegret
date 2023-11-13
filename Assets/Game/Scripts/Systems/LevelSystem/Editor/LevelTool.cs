using Game.Systems.GameSystem;
using Game.Systems.GoalSystem;

using ModestTree;

using StarSmithGames.Core;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Systems.LevelSystem
{
	[EditorTool("Level Tool")]
	public class LevelTool : EditorTool
	{
		private GUIContent toolContent;
		public override GUIContent toolbarIcon => toolContent ?? (toolContent = new GUIContent
		{
			image = (Texture)EditorGUIUtility.Load("GameEditor/Level.png"),
			text = "Place Objects Tool",
			tooltip = "Place Objects Tool"
		});

		private VisualElement toolRootElement;
		private ObjectField prefabObjectField;
		private ObjectField levelAssetField;

		private bool receivedClickDownEvent;
		private bool receivedClickUpEvent;

		private bool HasPlaceableObject => prefabObjectField?.value != null;

		public override void OnActivated()
		{
			//Create the UI
			toolRootElement = new VisualElement();
			toolRootElement.style.width = 200;
			var backgroundColor = EditorGUIUtility.isProSkin
				? new Color(0.21f, 0.21f, 0.21f, 0.8f)
				: new Color(0.8f, 0.8f, 0.8f, 0.8f);
			toolRootElement.style.backgroundColor = backgroundColor;
			toolRootElement.style.marginLeft = 10f;
			toolRootElement.style.marginBottom = 10f;
			toolRootElement.style.paddingTop = 5f;
			toolRootElement.style.paddingRight = 5f;
			toolRootElement.style.paddingLeft = 5f;
			toolRootElement.style.paddingBottom = 5f;

			var titleLabel = new Label("Place Objects Tool");
			titleLabel.style.unityTextAlign = TextAnchor.MiddleLeft;

			var levelLabel = new Label("Level");
			levelLabel.style.unityTextAlign = TextAnchor.MiddleLeft;

			prefabObjectField = new ObjectField { allowSceneObjects = true, objectType = typeof(GameObject) };
			levelAssetField = new ObjectField { allowSceneObjects = false, objectType = typeof(LevelConfig) };

			toolRootElement.Add(titleLabel);
			toolRootElement.Add(prefabObjectField);
			toolRootElement.Add(levelLabel);
			toolRootElement.Add(levelAssetField);
			toolRootElement.Add(new Button(OnButtonClicked) { text = "Refresh Level" });

			var sv = SceneView.lastActiveSceneView;
			sv.rootVisualElement.Add(toolRootElement);
			sv.rootVisualElement.style.flexDirection = FlexDirection.ColumnReverse;

			LoadLevel();

			SceneView.beforeSceneGui += BeforeSceneGUI;
		}

		public override void OnWillBeDeactivated()
		{
			toolRootElement?.RemoveFromHierarchy();
			SceneView.beforeSceneGui -= BeforeSceneGUI;
		}

		public override void OnToolGUI(EditorWindow window)
		{
			//If we're not in the scene view, we're not the active tool, we don't have a placeable object, exit.
			if (!(window is SceneView))
				return;

			if (!ToolManager.IsActiveTool(this))
				return;

			if (!HasPlaceableObject)
				return;

			//Draw a positional Handle.
			Handles.DrawWireDisc(GetCurrentMousePositionInScene(), Vector3.up, 0.5f);

			//If the user clicked, clone the selected object, place it at the current mouse position.
			if (receivedClickUpEvent)
			{
				var newObject = prefabObjectField.value;

				GameObject newObjectInstance;
				if (PrefabUtility.IsPartOfAnyPrefab(newObject))
				{
					var prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(newObject);
					var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
					newObjectInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
				}
				else
				{
					newObjectInstance = Instantiate((GameObject)newObject);
				}

				newObjectInstance.transform.position = GetCurrentMousePositionInScene();

				Undo.RegisterCreatedObjectUndo(newObjectInstance, "Place new object");

				receivedClickUpEvent = false;
			}

			//Force the window to repaint.
			window.Repaint();
		}

		private void BeforeSceneGUI(SceneView sceneView)
		{
			if (!ToolManager.IsActiveTool(this))
				return;

			if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
			{
				ShowMenu();
				Event.current.Use();
			}

			if (!HasPlaceableObject)
			{
				receivedClickDownEvent = false;
				receivedClickUpEvent = false;
			}
			else
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
				{
					receivedClickDownEvent = true;
					Event.current.Use();
				}

				if (receivedClickDownEvent && Event.current.type == EventType.MouseUp && Event.current.button == 0)
				{
					receivedClickDownEvent = false;
					receivedClickUpEvent = true;
					Event.current.Use();
				}
			}
		}

		private Vector3 GetCurrentMousePositionInScene()
		{
			Vector3 mousePosition = Event.current.mousePosition;
			var placeObject = HandleUtility.PlaceObject(mousePosition, out var newPosition, out var normal);
			return placeObject ? newPosition : HandleUtility.GUIPointToWorldRay(mousePosition).GetPoint(10);
		}

		private void ShowMenu()
		{
			var picked = HandleUtility.PickGameObject(Event.current.mousePosition, true);
			if (!picked) return;

			var menu = new GenericMenu();
			menu.AddItem(new GUIContent($"Pick {picked.name}"), false, () => { prefabObjectField.value = picked; });
			menu.ShowAsContext();
		}

		private void LoadLevel()
		{
			var config = AssetDatabaseExtensions.LoadAsset<GameplayConfig>();
			var level = config.GetLevelByScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
			levelAssetField.value = level;
		}

		private void OnButtonClicked()
		{
			Assert.IsNotNull(levelAssetField.value);

			var config = levelAssetField.value as LevelConfig;
			
			var goals = FindObjectsByType<GoalModel>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
			var groups = goals.GroupBy((x) => x.goal.config);

			List<GoalItem> items = new();
			foreach (var group in groups)
			{
				int count = 0;

				foreach (var item in group)
				{
					count += item.goal.count;
				}

				items.Add(new GoalItem()
				{
					config = group.Key,
					count = count,
				});
			}

			config.primaryGoals = items;

			EditorUtility.SetDirty(config);
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Game
{
	public class ReferenceFinder : EditorWindow
	{
		private List<string> _foundObjects = new();
		private List<int> _foundObjectsCount = new();

		[SerializeField]
		private List<Object> foundGameObjects;

		[SerializeField]
		private List<int> foundGameObjectsCount;

		private int _currentProgress;
		private int _filesCount;
		private string _fileName;
		private bool _customSearch;
		private Vector2 _scrollPosition;

		private List<CancellationTokenSource> _tasks = new();

		private readonly Dictionary<string, bool> _availableTypes = new()
	{
		{ "*.prefab", true },
		{ "*.mat", true },
		{ "*.unity", true },
		{ "*.asset", true },
		{ "*.controller", false },
		{ "*.anim", false }
	};

		private string _guid;
		private string _customText = string.Empty;

		private void SetUpSearch(string guid)
		{
			_guid = guid;
		}

		private void StartSearch(string guid)
		{
			_tasks.ForEach(task => task.Cancel());
			_tasks.ForEach(task => task.Dispose());
			_tasks = new List<CancellationTokenSource>();

			if (string.IsNullOrEmpty(guid))
				return;

			_guid = guid;
			_currentProgress = 0;
			_foundObjects = new List<string>();
			_foundObjectsCount = new List<int>();
			foundGameObjects = new List<Object>();
			foundGameObjectsCount = new List<int>();
			List<string> files = new List<string>();

			foreach (KeyValuePair<string, bool> availableType in _availableTypes)
			{
				if (availableType.Value)
					files.AddRange(Directory.GetFiles("Assets/", availableType.Key, SearchOption.AllDirectories).ToList());
			}

			_filesCount = files.Count;
			List<List<string>> chunkPaths = Split(files, SystemInfo.processorCount);

			for (var i = 0; i < chunkPaths.Count; i++)
			{
				// Debug.LogError(dividedPaths[i].Count);

				var list = chunkPaths[i];
				var id = i + 1;

				var cancellationTokenSource = new CancellationTokenSource();
				var cancellationToken = cancellationTokenSource.Token;
				var task = new Task(() => FindReferences(list, cancellationToken), cancellationToken);
				_tasks.Add(cancellationTokenSource);
				task.Start();
			}

			// for (var i = 0; i < tasks.Count; i++)
			// {
			//     tasks[i].Wait();
			// }

			// Repaint();

			// Debug.LogError("COMPLETE");
		}

		private static List<List<T>> Split<T>(List<T> collection, int chunkCount)
		{
			int size = collection.Count / chunkCount;

			List<List<T>> chunks = new List<List<T>>();

			for (var i = 0; i < chunkCount; i++)
			{
				chunks.Add(collection.Skip(i * size).Take(i < chunkCount - 1 ? size : collection.Count - i * size)
					.ToList());
			}

			return chunks;
		}

		private void FindReferences(List<string> pathList, CancellationToken cancellationToken)
		{
			foreach (var file in pathList)
			{
				// FileName = file;
				var streamReader = new StreamReader(file);

				var stream = streamReader.ReadToEnd();

				var count = CountInString(stream, _guid);
				if (count > 0)
				{
					_foundObjects.Add(file);
					_foundObjectsCount.Add(count);
				}

				streamReader.Dispose();

				_currentProgress++;

				if (cancellationToken.IsCancellationRequested)
				{
					// another thread decided to cancel
					//Debug.LogError("task canceled");
					break;
				}
			}

			GC.Collect();
			// Debug.LogError("Task Finished: " + id);
			// Debug.LogError("CurrentProgress: " + CurrentProgress);
		}

		private int CountInString(string str, string value)
		{
			var count = 0;
			if (String.IsNullOrEmpty(value))
			{
				return count;
			}

			for (var index = 0; ; index += value.Length)
			{
				index = str.IndexOf(value, index, StringComparison.Ordinal);
				if (index == -1)
				{
					return count;
				}

				count++;
			}
		}

		void Update()
		{
			// Debug.LogError(CurrentProgress+ " == " + FilesCount);
			if (_currentProgress % 100 == 0 || _currentProgress == _filesCount)
				Repaint();
		}

		void OnGUI()
		{
			minSize = maxSize = new Vector2(800, 600);

			//GUILayout.BeginArea(new Rect(0, 0, Screen.width, 50));
			GUILayout.BeginHorizontal("box");
			{
				GUILayout.BeginVertical();
				{
					_customSearch = GUILayout.Toggle(_customSearch, "Text Search");
					if (_customSearch)
					{
						_customText = GUILayout.TextArea(_customText);
					}
					else
					{
						_guid = GUILayout.TextArea(_guid);
					}
				}
				GUILayout.EndVertical();

				GUILayout.BeginVertical();
				{
					_availableTypes["*.prefab"] = GUILayout.Toggle(_availableTypes["*.prefab"], "Prefabs");
					_availableTypes["*.mat"] = GUILayout.Toggle(_availableTypes["*.mat"], "Materials");
					_availableTypes["*.unity"] = GUILayout.Toggle(_availableTypes["*.unity"], "Scenes");
					_availableTypes["*.asset"] = GUILayout.Toggle(_availableTypes["*.asset"], "Assets");
					_availableTypes["*.anim"] = GUILayout.Toggle(_availableTypes["*.anim"], "Anims");
					_availableTypes["*.controller"] = GUILayout.Toggle(_availableTypes["*.controller"], "Controllers");
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(20);

			if (GUILayout.Button("Search", GUILayout.Width(150), GUILayout.Height(50)))
			{
				StartSearch(_customSearch ? _customText : _guid);
			}

			//GUILayout.EndArea();

			GUILayout.Space(20);

			//float progress = FilesCount == 0 ? 0f : CurrentProgress * 100f / FilesCount;
			//GUILayout.Label("Progress: " + progress + "%");
			GUILayout.Label("Progress: " + _currentProgress + " / " + _filesCount);
			// GUILayout.Label("File: " + FileName);

			GUILayout.Space(20);

			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, true, true, GUILayout.Width(Screen.width),
				GUILayout.Height(Screen.height - 50));
			lock (_foundObjects)
			{
				if (_foundObjects.Count > 0 && foundGameObjects != null && _foundObjects.Count - foundGameObjects.Count > 0)
				{
					for (int i = foundGameObjects.Count; i < _foundObjects.Count; i++)
					{
						Object newObject = AssetDatabase.LoadAssetAtPath<Object>(_foundObjects[i]);
						if (foundGameObjects.Contains(newObject))
						{
							continue;
						}

						lock (_foundObjectsCount)
						{
							foundGameObjectsCount.Add(_foundObjectsCount[i]);
						}

						foundGameObjects.Add(newObject);
					}
				}
			}

			if (foundGameObjects != null)
			{
				for (var i = 0; i < foundGameObjects.Count; i++)
				{
					var foundGameObject = foundGameObjects[i];
					if (foundGameObject == null)
					{
						continue;
					}

					GUILayout.BeginHorizontal();

					EditorGUILayout.ObjectField(foundGameObject.name, foundGameObject, foundGameObject.GetType(), true);
					GUILayout.Label(foundGameObjectsCount[i].ToString());

					GUILayout.EndHorizontal();
				}
			}

			GUILayout.EndScrollView();
		}


		[MenuItem("Assets/Find All References", false, 25)]
		public static void ShowSearch()
		{
			Object mainObject = Selection.objects.First();

			string path = AssetDatabase.GetAssetPath(mainObject);
			string guid = AssetDatabase.AssetPathToGUID(path);

			ReferenceFinder popup = GetWindow<ReferenceFinder>(true, "Reference Finder", true);
			popup.SetUpSearch(guid);
		}

		private void OnDestroy()
		{
			_tasks.ForEach(task => task.Cancel());
			_tasks.ForEach(task => task.Dispose());
		}
	}
}
using Game.Managers.GameManager;
using Game.Systems.StorageSystem;
using Game.UI;

using StarSmithGames.Go;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;

namespace Game.Services
{
	public class ViewService
	{
		public ViewRegistrator ViewDialogRegistrator
		{
			get
			{
				if(viewDialogRegistrator == null)
				{
					viewDialogRegistrator = new ViewRegistrator();
				}

				return viewDialogRegistrator;
			}
		}
		private ViewRegistrator viewDialogRegistrator;

		[Inject] private DiContainer container;
		[Inject] private List<ViewBase> dialogs;
		[Inject] private GameManager gameManager;
		[Inject] private GameData gameData;

		public T CreateDialogIfNotExist<T>(Transform customParent = null)
			where T : MonoBehaviour, IView
		{
			if (!ViewDialogRegistrator.ContainsType<T>())
			{
				T prefab = dialogs.OfType<T>().First();
				if (prefab != null)
				{
					var dialog = container.InstantiatePrefabForComponent<T>(prefab, customParent ? customParent : GetCurrentUIParent());
					ViewDialogRegistrator.Registrate(dialog);
					return dialog;
				}

				throw new Exception($"[ViewService] Prefabs DOESN'T CONTAINS {typeof(T)} ERROR");
			}

			return ViewDialogRegistrator.GetAs<T>();
		}

		public void TryShowDialog<T>()
			where T : MonoBehaviour, IView
		{
			CreateDialogIfNotExist<T>().Show();
		}

		private Transform GetCurrentUIParent()
		{
			return gameManager.IsMenu ? GetMenuRoot() : null;
		}

		private Transform GetMenuRoot()
		{
			return gameData.IntermediateData.RootMenu.dynamicCanvas.dialogsRoot;
		}
	}
}
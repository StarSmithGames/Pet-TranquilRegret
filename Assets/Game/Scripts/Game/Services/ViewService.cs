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
		[Inject] private StorageSystem gameData;

		public T CreateDialogIfNotExist<T>(Transform customParent = null)
			where T : MonoBehaviour, IView
		{
			if (!ViewDialogRegistrator.ContainsType<T>())
			{
				T prefab = dialogs.OfType<T>().First();
				if (prefab != null)
				{
					var root = GetCurrentUIParent();
					var dialog = root.Container.InstantiatePrefabForComponent<T>(prefab, customParent ? customParent : root.GetDialogsRoot());
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

		public bool IsSafeDialogShowig()
		{
			return false;
		}

		private UIRoot GetCurrentUIParent()
		{
			return gameManager.IsMenu ? gameData.IntermediateData.RootMenu : gameData.IntermediateData.RootGame;
		}
	}
}
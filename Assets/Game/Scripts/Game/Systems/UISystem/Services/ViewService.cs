using Game.Managers.GameManager;
using Game.Systems.StorageSystem;
using Game.UI;

using StarSmithGames.Go;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;

namespace Game.UI.Services
{
	public sealed class ViewService
	{
		public ViewRegistrator ViewRegistrator
		{
			get
			{
				if(_viewDialogRegistrator == null)
				{
					_viewDialogRegistrator = new();
				}

				return _viewDialogRegistrator;
			}
		}
		private ViewRegistrator _viewDialogRegistrator;

		private List<ViewBase> _dialogs;
		private UIRoot _root;

		public ViewService(
			UISettings settings,
			UIRoot root
			)
		{
			_dialogs = settings.Dialogs;
			_root = root;
		}

		public T CreateDialogIfNotExist<T>(Transform customParent = null)
			where T : MonoBehaviour, IView
		{
			if (!ViewRegistrator.ContainsType<T>())
			{
				T prefab = _dialogs.OfType<T>().First();
				if (prefab != null)
				{
					var dialog = _root.Container.InstantiatePrefabForComponent<T>(prefab, customParent ? customParent : _root.GetDialogsRoot());
					return dialog;
				}

				throw new Exception($"[ViewService] Prefabs DOESN'T CONTAINS {typeof(T)} ERROR");
			}

			return ViewRegistrator.GetAs<T>();
		}

		public void TryShowDialog<T>()
			where T : MonoBehaviour, IView
		{
			CreateDialogIfNotExist<T>().Show();
		}
	}
}
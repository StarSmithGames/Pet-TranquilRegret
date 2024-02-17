using StarSmithGames.Go;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Game.Systems.UISystem
{
	public sealed class ViewAggregator : IDisposable
	{
		private ViewRegistrator _viewRegistrator;

		private readonly ViewCreator _viewCreator;
		private readonly List< ViewBase > _views;

		public ViewAggregator(
			ViewCreator viewCreator,
			List< ViewBase > views
			)
		{
			_viewCreator = viewCreator ?? throw new ArgumentNullException( nameof(viewCreator) );
			_views = views ?? throw new ArgumentNullException( nameof(views) );

			_viewRegistrator = new();
		}
		
		public void Dispose()
		{
			for ( int i = 0; i < _viewRegistrator.registers.Count; i++ )
			{
				ViewDisposedHandler( (IUIView)_viewRegistrator.registers[ i ] );
			}
		}

		public T CreateIfNotExist<T>()
			where T : MonoBehaviour, IUIView
		{
			if (!_viewRegistrator.ContainsType<T>())
			{
				T prefab = _views.OfType<T>().FirstOrDefault();
				if (prefab != null)
				{
					var view = _viewCreator.InstantiatePrefabForComponent< T >( prefab );
					_viewRegistrator.Registrate( view );
					view.OnDisposed += ViewDisposedHandler;
					return view;
				}

				throw new Exception($"[UISystem] DOESN'T CONTAINS TYPE: {typeof(T)}");
			}

			return _viewRegistrator.GetAs<T>();
		}

		public void ShowOrCreateIfNotExist<T>()
			where T : MonoBehaviour, IUIView
		{
			CreateIfNotExist<T>().Show();
		}

		private void ViewDisposedHandler( IUIView view )
		{
			view.OnDisposed -= ViewDisposedHandler;
			_viewRegistrator.UnRegistrate( view );
		}
	}
}
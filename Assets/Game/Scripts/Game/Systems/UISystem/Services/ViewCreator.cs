using System;
using UnityEngine;
using Zenject;

namespace Game.Systems.UISystem
{
	public sealed class ViewCreator
	{
		private readonly DiContainer _container;
		private readonly Transform _root;

		public ViewCreator(
			DiContainer container,
			Transform root
			)
		{
			_container = container ?? throw new ArgumentNullException( nameof(container) );
			_root = root ?? throw new ArgumentNullException( nameof(root) );
		}

		public T Instantiate< T >()
		{
			return _container.Instantiate< T >();
		}

		public T InstantiatePrefabForComponent< T >( UnityEngine.Object prefab, Transform parentTransform = null )
		{
			return _container.InstantiatePrefabForComponent< T >( prefab, parentTransform ?? _root );
		}
	}
}
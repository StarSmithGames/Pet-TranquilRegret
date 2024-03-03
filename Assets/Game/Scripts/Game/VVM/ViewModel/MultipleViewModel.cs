using Game.VVM.Services;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.VVM
{
	public abstract class MultipleViewModel< T > : ViewModel< T >
		where T : View
	{
		protected ViewModelService _innerViewModelService;
		protected List< IViewModel > _viewModels = new();

		public MultipleViewModel(
			DiContainer diContainer
			)
		{
			_innerViewModelService = new( diContainer );
		}

		public override void Dispose()
		{
			_viewModels.Clear();
			
			base.Dispose();
		}
		
		protected override void OnViewCreated()
		{
			_viewModels.Clear();
			var _runtimeViewModels = GetRuntimeViewModels();
			for ( int i = 0; i < _runtimeViewModels.Count; i++ )
			{
				_viewModels.Add( _innerViewModelService.Create( _runtimeViewModels[ i ] ) );
			}
		}

		protected abstract List< Type > GetRuntimeViewModels();
	}
}
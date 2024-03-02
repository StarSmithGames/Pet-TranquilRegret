using Game.VVM.Services;
using System;
using System.Collections.Generic;
using Zenject;

namespace Game.VVM
{
	public abstract class MultipleViewModel< T > : ViewModel< T >
		where T : View
	{
		protected ViewModelService _innerViewModelService;

		public MultipleViewModel(
			DiContainer diContainer
			)
		{
			_innerViewModelService = new( diContainer );
		}

		protected override void OnViewCreated()
		{
			var _runtimeViewModels = GetRuntimeViewModels();
			for ( int i = 0; i < _runtimeViewModels.Count; i++ )
			{
				_innerViewModelService.Create( _runtimeViewModels[ i ] );
			}
		}

		protected abstract List< Type > GetRuntimeViewModels();
	}
}
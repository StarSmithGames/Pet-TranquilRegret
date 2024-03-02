using StarSmithGames.Core;
using System;
using Zenject;

namespace Game.VVM.Services
{
	public sealed class ViewModelService : Registrator< IViewModel >, IDisposable
	{
        private readonly DiContainer _diContainer;
        
        public ViewModelService( DiContainer diContainer )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
        }
        
        public void Dispose()
        {
            for ( int i = 0; i < registers.Count; i++ )
            {
	            registers[i].Dispose();
            }
        }
        
        public T GetOrCreateIfNotExist< T >()
            where T : class, IViewModel
        {
            if ( !ContainsType< T >() )
            {
                var model = _diContainer.Instantiate< T >();
                model.OnDisposed += OnViewModelDisposed;
                Registrate( model );
                model.Initialize();
                return model;
            }

            return GetAs< T >();
        }
        
        public IViewModel Create( Type t )
        {
            var model = _diContainer.Instantiate( t ) as IViewModel;
            model.OnDisposed += OnViewModelDisposed;
            Registrate( model );
            model.Initialize();
            return model;
        }

        public void CreateAndShowIfNotExist< T >( Action callback = null )
            where T : class, IViewModel
        {
            var model = GetOrCreateIfNotExist< T >();
            model.ShowView( callback );
        }

        private void OnViewModelDisposed( IViewModel viewModel )
        {
            viewModel.OnDisposed -= OnViewModelDisposed;
            UnRegistrate( viewModel );
        }
	}
}
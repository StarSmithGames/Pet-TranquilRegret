using System;

namespace Game.VVM
{
	public abstract class ViewModel< T > : IViewModel
		where T : View
	{
		public event Action< IViewModel > OnDisposed;
		
		public T ModelView { get; protected set; }
		
		/// <summary>
		/// Call ViewModelService on Create
		/// </summary>
		public virtual void Initialize() { }
		

		protected virtual T CreateView()
		{
			if ( ModelView != null )
			{
				throw new Exception( "View Error" );
			}

			ModelView = GetView();
            
			SubscribeView();
			OnViewCreated();
			return ModelView;
		}

		protected virtual void DisposeView()
		{
			if ( ModelView != null )
			{
				UnSubscribeView();
				ModelView.Dispose();
			}
			ModelView = null;
			OnViewDisposed();
		}
		
		protected abstract T GetView();
		
		protected virtual void SubscribeView()
		{
		}
        
		protected virtual void UnSubscribeView()
		{
			if ( ModelView != null )
			{

			}
		}

		
		public void EnableView( bool trigger )
		{
			if ( ModelView == null )
			{
				CreateView();
			}
			ModelView.Enable( trigger );
		}

		public void ShowView( Action callback = null )
		{
			if ( ModelView == null )
			{
				CreateView();
			}
			ModelView.Show( callback );
		}
        
		public void HideView( Action callback = null )
		{
			if ( ModelView == null )
			{
				CreateView();
			}
			ModelView.Hide( () =>
			{
				callback?.Invoke();
				DisposeView();
			} );
		}
        
		public void HideViewAndDispose( Action callback = null )
		{
			HideView( () =>
			{
				Dispose();
				callback?.Invoke();
			} );
		}
		
		public virtual void Dispose()
		{
			DisposeView();
            
			OnDisposed?.Invoke( this );
		}
		
		protected virtual void OnViewCreated() { }
		protected virtual void OnViewDisposed() { }
	}
}
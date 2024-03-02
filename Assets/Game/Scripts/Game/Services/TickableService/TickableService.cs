using Cysharp.Threading.Tasks;
using StarSmithGames.Core.Utils;
using System;
using System.Threading;

namespace Game.Services.TickableService
{
    public sealed class TickableService
    {
        public event Action OnUpdate;
        public event Action OnSecondsTick;

        public TickableService()
        {
	        Update( ThreadingUtils.QuitToken );
            SecondsTick( ThreadingUtils.QuitToken );
        }

        private async UniTask Update( CancellationToken token )
        {
	        bool isCanceled = false;
            
	        while ( !isCanceled )
	        {
		        OnUpdate?.Invoke();
                
		        isCanceled = await UniTask.Yield( token ).SuppressCancellationThrow();
	        }
        }
        
        private async UniTask FrameUpdate( CancellationToken token )
        {
            bool isCanceled = false;
            
            while ( !isCanceled )
            {
                OnUpdate?.Invoke();
                
                isCanceled = await UniTask.NextFrame( token ).SuppressCancellationThrow();
            }
        }
        
        private async UniTask SecondsTick( CancellationToken token )
        {
            bool isCanceled = false;
            
            while ( !isCanceled )
            {
                OnSecondsTick?.Invoke();
                
                isCanceled = await UniTask.WaitForSeconds( 1f, cancellationToken: token ).SuppressCancellationThrow();
            }
        }
    }
}
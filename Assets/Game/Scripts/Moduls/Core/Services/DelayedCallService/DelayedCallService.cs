using Cysharp.Threading.Tasks;

using StarSmithGames.Core.Utils;

using System;
using System.Threading;

namespace Company.Module.Services.DelayedCallService
{
	public sealed class DelayedCallService : IDelayedCallService
	{
		public void DelayedCallAsync(float delay, Action callback)
		{
			_ = Call(delay, callback, ThreadingUtils.QuitToken);
		}

		public void DelayedCall(float delay, Action callback)
		{

		}

		private async UniTask Call(float delay, Action callback, CancellationToken token)
		{
			await UniTask.WaitForSeconds(delay, cancellationToken: token);
			callback?.Invoke();
		}
	}
}
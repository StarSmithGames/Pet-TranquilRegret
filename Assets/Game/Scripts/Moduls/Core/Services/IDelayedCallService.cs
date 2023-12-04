using System;

namespace Company.Module.Services.DelayedCallService
{
	public interface IDelayedCallService
	{
		void DelayedCallAsync(float delay, Action callback);
		void DelayedCall(float delay, Action callback);
	}
}
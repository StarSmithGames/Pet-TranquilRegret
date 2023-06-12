using System;
using UnityEngine;

namespace Game
{
	public static class TimeExtensions
	{
		public static string GetTimerFormat(TimeSpan timeSpan)
		{
			return $"{(timeSpan.Minutes < 10 ? $"0{timeSpan.Minutes}" : timeSpan.Minutes)}:{(timeSpan.Seconds < 10 ? $"0{timeSpan.Seconds}" : timeSpan.Seconds)}";
		}
	}
}
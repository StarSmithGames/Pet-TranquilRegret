using StarSmithGames.Core;
using System;
using UnityEngine;

namespace Game.Systems.StorageSystem
{
	public abstract class BoosterData : IObservableValue
	{
		public event Action onChanged;
		[ SerializeField ] private int itemsCount;

		public int useCount;

		public int ItemsCount
		{
			get => itemsCount;
			set
			{
				itemsCount = value;

				onChanged?.Invoke();
			}
		}
		
		public bool IsEmpty => itemsCount == 0;
	}
}
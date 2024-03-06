using System;
using UnityEngine;

namespace Game.Entity
{
	public abstract class EntityObject : MonoBehaviour
	{
		public event Action< EntityObject > OnDisposed;
		
		public virtual void Dispose()
		{
			OnDisposed?.Invoke( this );
		}
	}
}
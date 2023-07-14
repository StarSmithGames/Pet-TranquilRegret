using StarSmithGames.IoC;

using UnityEngine;

namespace Game.VFX.Markers
{
	public interface IPointer : IPoolable
	{
		Transform Transform { get; }
	}

	public abstract class Pointer : PoolableObject, IPointer
	{
		public virtual Transform Transform => transform;
	}
}
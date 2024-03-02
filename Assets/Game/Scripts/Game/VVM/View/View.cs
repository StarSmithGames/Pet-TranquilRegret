using System;
using UnityEngine;

namespace Game.VVM
{
	public abstract class View : MonoBehaviour
	{
		public virtual void Dispose()
		{
			
		}
		
		public virtual void Enable( bool trigger )
		{
			
		}

		public virtual void Show( Action callback = null )
		{
			
		}

		public virtual void Hide( Action callback = null )
		{
			
		}
	}
}
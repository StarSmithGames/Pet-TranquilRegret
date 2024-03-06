using Game.Scripts.Extensions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Systems.CollisionSystem
{
	public abstract class ColliderCaster : MonoBehaviour  
	{
		public LayerMask Mask = -1;
		public List< Collider > ExceptColliders = new();
		
		public readonly Observer< Collider > Observer = new();

		public void Enable( bool trigger )
		{
			gameObject.SetActive( trigger );
			
			if ( !trigger )
			{
				Observer.Observers.Clear();
			}
		}

		protected virtual bool IsObservable( Collider other )
		{
			if ( !LayersExtensions.Contains( Mask.value, other.gameObject.layer ) ) return false;
			if ( ExceptColliders.Contains( other ) ) return false;

			return true;
		}

		private void OnTriggerEnter( Collider other )
		{
			if ( !IsObservable( other ) ) return;
			
			if ( !Observer.Contains( other ) )
			{
				Observer.AddObserver( other );
			}
		}

		private void OnTriggerExit( Collider other )
		{
			if ( !IsObservable( other ) ) return;

			if ( Observer.Contains( other ) )
			{
				Observer.RemoveObserver( other );
			}
		}

#if UNITY_EDITOR
		[Button( DirtyOnClick = true )]
		private void FillAll()
		{
			ExceptColliders = transform.root.GetComponentsInChildren< Collider >().ToList();
		}
#endif
	}
}
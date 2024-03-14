using Game.Entity;
using Game.Scripts.Extensions;
using Game.Systems.CombatSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Systems.CollisionSystem
{
	public sealed class DamagableEntityColliderCaster : MonoBehaviour
	{
		public LayerMask Mask = -1;
		public List< Collider > ExceptColliders = new();
		
		public readonly Observer< EntityObject > Observer = new();
		
		public void Enable( bool trigger )
		{
			gameObject.SetActive( trigger );
			
			if ( !trigger )
			{
				Observer.Observers.Clear();
			}
		}
		
		private bool IsObservable( EntityObject entity, Collider other )
		{
			if ( !LayersExtensions.Contains( Mask.value, other.gameObject.layer ) ) return false;
			if ( ExceptColliders.Contains( other ) ) return false;
			
			return entity is IDamageable;
		}
		
		private void OnTriggerEnter( Collider other )
		{
			EntityObject entity = other.GetComponentInParent< EntityObject >();
			
			if ( !IsObservable( entity, other ) ) return;
			
			if ( !Observer.Contains( entity ) )
			{
				entity.OnDisposed += EntityDesposableHandler;
				Observer.AddObserver( entity );
			}
		}

		private void OnTriggerExit( Collider other )
		{
			EntityObject entity = other.GetComponentInParent< EntityObject >();
			
			if ( !IsObservable( entity, other ) ) return;
			
			if ( Observer.Contains( entity ) )
			{
				Observer.RemoveObserver( entity );
				
				entity.OnDisposed -= EntityDesposableHandler;
			}
		}

		private void EntityDesposableHandler( EntityObject entity )
		{
			if ( Observer.Contains( entity ) )
			{
				Observer.RemoveObserver( entity );
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
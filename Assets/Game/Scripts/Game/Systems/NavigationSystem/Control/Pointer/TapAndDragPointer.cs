using UnityEngine.EventSystems;

namespace Game.Systems.NavigationSystem
{
	public abstract class TapAndDragPointer : TapPointer, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public bool IsDragging { get; protected set; }

		public override void OnPointerUp( PointerEventData eventData )
		{
			if ( !IsDragging )
			{
				base.OnPointerUp( eventData );
			}
		}

		public virtual void OnBeginDrag( PointerEventData eventData )
		{
			IsDragging = true;
		}
		
		public virtual void OnDrag( PointerEventData eventData )
		{
		}

		public virtual void OnEndDrag( PointerEventData eventData )
		{
			IsDragging = false;
		}
	}
}
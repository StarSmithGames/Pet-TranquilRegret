using System;
using System.Collections.Generic;

public class Observer< T >
{
	public event Action onCollectionChanged;
	public event Action< T > onItemAdded;
	public event Action< T > onItemRemoved;

	public readonly List< T > Observers = new();

	//Add observer to the list
	public virtual void AddObserver( T observer, bool notify = true )
	{
		Observers.Add( observer );

		if ( !notify ) return;
		
		onItemAdded?.Invoke( observer );
		onCollectionChanged?.Invoke();
	}

	public virtual void AddObservers( IEnumerable< T > observers, bool notify = true )
	{
		Observers.AddRange( observers );
		
		if ( !notify ) return;
		
		onCollectionChanged?.Invoke();
	}

	//Remove observer from the list
	public virtual void RemoveObserver( T observer, bool notify = true )
	{
		Observers.Remove( observer );
		
		if ( !notify ) return;

		onItemRemoved?.Invoke( observer );
		onCollectionChanged?.Invoke();
	}

	public virtual void RemoveObservers( IEnumerable< T > observers, bool notify = true )
	{
		foreach ( var observer in observers )
		{
			Observers.Remove( observer );
		}
		
		if ( !notify ) return;
		
		onCollectionChanged?.Invoke();
	}

	public virtual bool Contains( T observer )
	{
		return Observers.Contains( observer );
	}
}

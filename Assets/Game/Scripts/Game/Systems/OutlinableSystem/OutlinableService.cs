using DG.Tweening;
using EPOOutline;
using System;
using System.Collections.Generic;

namespace Game.Services
{
	public sealed class OutlinableService
	{
		private readonly List< Outlinable > _outlinables;
		
		private readonly OutlinableManager _outlinableManager;
		
		public OutlinableService(
			OutlinableManager outlinableManager
			)
		{
			_outlinableManager = outlinableManager ?? throw new ArgumentNullException( nameof(outlinableManager) );

			_outlinables = _outlinableManager.Observers;
		}
		
		public void Enable( bool trigger )
		{
			for ( int i = 0; i < _outlinables.Count; i++ )
			{
				_outlinables[ i ].enabled = trigger;
			}
		}

		public void SetOutlineData( OutlineData data )
		{
			for ( int i = 0; i < _outlinables.Count; i++ )
			{
				_outlinables[ i ].SetData( data );
			}
		}
		
		public void ShowAll()
		{
			for ( int i = 0; i < _outlinables.Count; i++ )
			{
				// _outlinables[ i ].Properties.outlineParameters.;
			}
		}

		public void HideAll()
		{
			
		}
	}
}
using StarSmithGames.Core;
using System;
using Zenject;

namespace Game.Systems.CombatSystem
{
	public sealed class AttackAggregator : Registrator< Attack >
	{
		private readonly DiContainer _diContainer;
        
		public AttackAggregator( DiContainer diContainer )
		{
			_diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
		}
		
		public T GetOrCreateIfNotExist< T >()
			where T : Attack
		{
			if ( !ContainsType< T >() )
			{
				var model = _diContainer.Instantiate< T >();
				Registrate( model );
				return model;
			}

			return GetAs< T >();
		}
	}
}
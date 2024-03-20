using Game.Systems.CombatSystem;
using System;
using UnityEngine;

namespace Game.Entity.CharacterSystem
{
	public sealed class CharacterCombat
	{
		private readonly CharacterView _view;
		private readonly AttackAggregator _attackAggregator;

		public CharacterCombat(
			CharacterView view,
			AttackAggregator attackAggregator
			)
		{
			_view = view ?? throw new ArgumentNullException( nameof(view) );
			_attackAggregator = attackAggregator ?? throw new ArgumentNullException( nameof(attackAggregator) );

			_view.FrontSphereCaster.Enable( true );
		}
		
		public void Attack()
		{
			var vfx = _attackAggregator.GetOrCreateIfNotExist< SlashAttack >().Execute();
			vfx.transform.position = _view.Points.Root.position + Vector3.up * 0.5f;
			vfx.transform.forward = _view.Points.Rotor.forward;
			vfx.Play();

			var damage = new Damage()
			{
				Amount = new( 0, 5 ),
				DamageType = DamageType.Slashing,
				AttackDirection = _view.Points.Rotor.forward
			};

			var list = _view.FrontSphereCaster.Observer.Observers;
			for ( int i = list.Count - 1; i >= 0; i-- )
			{
				var damageable = list[ i ];
				((IDamageable)damageable).TakeDamage( damage );
			}
		}
	}
}
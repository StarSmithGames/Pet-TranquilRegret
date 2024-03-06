using Game.Systems.CombatSystem;
using Game.Systems.ZoneSystem;
using UnityEngine.Events;
using Zenject;

namespace Game.Entity.CharacterSystem
{
	public class Character : EntityObject, IDieable, IZonable
	{
		public event UnityAction< IDieable > OnDied;
		
		public CharacterPresenter Presenter { get; private set; }

		[Inject]
		private void Construct(CharacterPresenter presenter)
		{
			Presenter = presenter;
		}
		
		public void TakeDamage( Damage damage ) => throw new System.NotImplementedException();

		public void Die()
		{
			gameObject.SetActive( false );
			
			OnDied?.Invoke( this );
			
			Dispose();
		}
	}
}
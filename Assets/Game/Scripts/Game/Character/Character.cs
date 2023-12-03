using Game.Systems.ZoneSystem;

using UnityEngine;

using Zenject;

namespace Game.Character
{
	public class Character : MonoBehaviour, IZonable
	{
		public CharacterPresenter Presenter { get; private set; }

		[Inject]
		private void Construct(CharacterPresenter presenter)
		{
			Presenter = presenter;
		}
	}
}
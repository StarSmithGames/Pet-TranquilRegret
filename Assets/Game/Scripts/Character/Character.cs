using UnityEngine;

using Zenject;

namespace Game.Character
{
	public class Character : MonoBehaviour
	{
		public CharacterPresenter Presenter { get; private set; }

		[Inject]
		private void Construct(CharacterPresenter presenter)
		{
			Presenter = presenter;
		}
	}
}
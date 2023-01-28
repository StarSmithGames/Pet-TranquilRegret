using Game.UI;

using UnityEngine;

using Zenject;

namespace Game.VFX.Markers
{
	public sealed class UIPointer : WindowPopupBasePoolable, IPointer
	{
		public Transform Transform => transform;

		public class Factory : PlaceholderFactory<UIPointer> { }
	}
}
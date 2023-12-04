using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.UI
{
	public abstract class UIRoot : MonoBehaviour
	{
		public DiContainer Container => container;

		[Inject] private DiContainer container;

		public abstract Transform GetDialogsRoot();
	}
}
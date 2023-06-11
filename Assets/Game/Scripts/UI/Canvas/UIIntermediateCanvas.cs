using Game.Managers.TransitionManager;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class UIIntermediateCanvas : UISubCanvas
	{
		[field: Space]
		[field: SerializeField] public UIFadeTransition FadeTransition { get; private set; }
	}
}
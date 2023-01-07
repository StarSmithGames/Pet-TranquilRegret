using Game.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Managers.TransitionManager
{
	public class TransitionManager
	{
		public bool IsInProccess => intermediateCanvas.FadeTransition.IsInProcess;

		private UIIntermediateCanvas intermediateCanvas;

		public TransitionManager(UIIntermediateCanvas intermediateCanvas)
		{
			this.intermediateCanvas = intermediateCanvas;
		}

		public void In(UnityAction callback = null)
		{
			intermediateCanvas.FadeTransition.Show(callback);
		}

		public void Out(UnityAction callback = null)
		{
			intermediateCanvas.FadeTransition.Hide(callback);
		}
	}

	public interface ITransitable : IShowable
	{
		void Terminate();
	}
}
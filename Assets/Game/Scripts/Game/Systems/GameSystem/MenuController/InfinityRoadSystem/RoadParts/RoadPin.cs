using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.VFX;
using System;
using UnityEngine;
using Zenject;

namespace Game.Systems.InfinityRoadSystem
{
	public sealed class RoadPin : MonoBehaviour
	{
		public bool IsInProcess { get; private set; }
		
		public float StepDelta = 1.5f;
		public float StepGap = 1f;
		
		private Vector3 _lastStepEmit;
		private int _stepDir = 1;
		
		[ Inject( Id = "StepPawVerticalPrint" ) ]
		private ParticalVFXFootStep.Factory _pawStepFactory;
		
		private void Step()
		{
			if (Vector3.Distance(_lastStepEmit, transform.position) > StepDelta)
			{
				var step = _pawStepFactory.Create();
				_stepDir *= -1;

				Vector3 toPosition = (transform.position - _lastStepEmit).normalized;
				float angleToPosition = Vector3.Angle(Vector3.up, toPosition);

				step.Play(new ParticleSystem.EmitParams()
				{
					position = transform.position + (transform.right * StepGap * _stepDir),
					rotation = angleToPosition,
				});

				_lastStepEmit = transform.position;
			}
		}

		public void Break()
		{
			transform.DOKill( true );
		}

		public void DoMoveInstant( Vector3 position, Action callback = null )
		{
			IsInProcess = true;
			
			DOTween.Sequence()
				.Append( transform.DOScale(0, 0.2f) )
				.AppendCallback(() => transform.position = position)
				.Append( transform.DOScale(1, 0.25f ).SetEase(Ease.OutBounce))
				.OnComplete( () =>
				{
					IsInProcess = false;
					callback?.Invoke();
				} );
		}
		
		public void DoMoveFirstTime( Vector3[] path, Action callback = null )
		{
			transform.position = new Vector3(0, -100, 0);
			DoMove( path, callback );
		}

		public void DoMove( Vector3[] path, Action callback = null )
		{
			IsInProcess = true;
			transform
				.DOPath( path, 1.5f ).OnUpdate( Step )
				.OnComplete( () =>
				{
					IsInProcess = false;
					callback?.Invoke();
				} );
		}
	}
}
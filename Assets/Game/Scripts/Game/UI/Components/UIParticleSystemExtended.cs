using System;
using UnityEngine;
using UnityEngine.UI;

[ ExecuteInEditMode ]
[ RequireComponent( typeof(CanvasRenderer), typeof(ParticleSystem) ) ]
public class UIParticleSystemExtended : MaskableGraphic
{
	[ Tooltip("Having this enabled run the system in LateUpdate rather than in Update making it faster but less precise (more clunky)" ) ]
	public bool fixedTime = true;

	public float Fps = 15f;

	public bool CheckStopAction;

	private CanvasGroup _canvasGroup;

	[ HideInInspector ][ SerializeField ] private Transform _cacheTransform;

	[ HideInInspector ][ SerializeField ] private ParticleSystem _cacheParticleSystem;

	private ParticleSystem _particleSystem;
	private ParticleSystem.Particle[] particles;
	private ParticleSystem.MainModule _mainModule;
	private ParticleSystem.TextureSheetAnimationModule _textureSheetAnimation;
	private ParticleSystemRenderer _particleRenderer;

	private UIVertex[] _quad = new UIVertex[4];
	private Vector4 _imageUV = Vector4.zero;

	private int _textureSheetAnimationFrames;
	private Vector2 _textureSheetAnimationFrameSize;

	private Material _currentMaterial;
	private Texture _currentTexture;

	private float _tick;
	private float _delta;

	public override Texture mainTexture
	{
		get { return _currentTexture; }
	}

	public override bool raycastTarget
	{
		get { return false; }
		set {}
	}

	protected bool Initialize()
	{
		// initialize members
		if ( _cacheTransform == null )
			_cacheTransform = transform;
		if ( _cacheParticleSystem == null )
			_cacheParticleSystem = GetComponent< ParticleSystem >();

		if ( _particleSystem == null )
		{
			_tick = 1f / Mathf.Min( Mathf.Max( Fps, 1f ), 60f );
			_delta = _tick;
			_particleSystem = _cacheParticleSystem;

			if ( _particleSystem == null )
				return false;

			_mainModule = _particleSystem.main;
			_mainModule.maxParticles = Mathf.Min( _mainModule.maxParticles, 100 );

			_particleRenderer = _particleSystem.GetComponent< ParticleSystemRenderer >();
			if ( _particleRenderer != null )
				_particleRenderer.enabled = false;

			if ( material == null )
			{
				var foundShader = Shader.Find( "IFly/UI/Particles/Additive" );
				material = new Material( foundShader );
			}

			_currentMaterial = material;
			if ( _currentMaterial && _currentMaterial.HasProperty( "_MainTex" ) )
			{
				_currentTexture = _currentMaterial.mainTexture;
				if ( _currentTexture == null )
					_currentTexture = Texture2D.whiteTexture;
			}

			material = _currentMaterial;
			// automatically set scaling
			_mainModule.scalingMode = ParticleSystemScalingMode.Hierarchy;

			particles = null;
		}

		if ( particles == null )
			particles = new ParticleSystem.Particle[_mainModule.maxParticles];

		_imageUV = new Vector4( 0, 0, 1, 1 );

		// prepare texture sheet animation
		_textureSheetAnimation = _particleSystem.textureSheetAnimation;
		_textureSheetAnimationFrames = 0;
		_textureSheetAnimationFrameSize = Vector2.zero;
		if ( _textureSheetAnimation.enabled )
		{
			_textureSheetAnimationFrames = _textureSheetAnimation.numTilesX * _textureSheetAnimation.numTilesY;
			_textureSheetAnimationFrameSize = new Vector2( 1f / _textureSheetAnimation.numTilesX,
				1f / _textureSheetAnimation.numTilesY );
		}

		return true;
	}

	protected override void Awake()
	{
		base.Awake();

		if ( !Initialize() )
			enabled = false;
	}

	protected override void OnDisable()
	{
		_canvasGroup = null;

		base.OnDisable();
	}

	protected override void OnTransformParentChanged()
	{
		base.OnTransformParentChanged();

		if ( _canvasGroup == null )
			_canvasGroup = GetComponentInParent< CanvasGroup >();
	}

	protected override void OnPopulateMesh( VertexHelper vh )
	{
#if UNITY_EDITOR
		if ( !Application.isPlaying )
		{
			if ( !Initialize() )
			{
				return;
			}
		}
#endif

		// prepare vertices
		vh.Clear();

		if ( !gameObject.activeInHierarchy || ( _canvasGroup != null && _canvasGroup.alpha <= 0f ) )
			return;

		Vector2 temp = Vector2.zero;
		Vector2 corner1 = Vector2.zero;
		Vector2 corner2 = Vector2.zero;
		// iterate through current particles
		int count = _particleSystem.GetParticles( particles );

		for ( int i = 0; i < count; ++i )
		{
			var particle = particles[ i ];

			// get particle properties
			Vector2 position = ( _mainModule.simulationSpace == ParticleSystemSimulationSpace.Local
				? particle.position
				: _cacheTransform.InverseTransformPoint( particle.position ) );

			float rotation = -particle.rotation * Mathf.Deg2Rad;
			float rotation90 = rotation + Mathf.PI / 2;
			Color32 color = particle.GetCurrentColor( _particleSystem );
			float size = particle.GetCurrentSize( _particleSystem ) * 0.5f;

			// apply scale
			if ( _mainModule.scalingMode == ParticleSystemScalingMode.Shape )
				position /= canvas.scaleFactor;

			// apply texture sheet animation
			Vector4 particleUV = _imageUV;
			if ( _textureSheetAnimation.enabled )
			{
				float time = 1 - ( particle.remainingLifetime / particle.startLifetime );
				float frameProgress = time;
				int frame = 0;
				var frameOverTime = _textureSheetAnimation.frameOverTime;

				if ( frameOverTime.curveMin != null )
					frameProgress = frameOverTime.curveMin.Evaluate( time );
				else if ( frameOverTime.curve != null )
					frameProgress = frameOverTime.curve.Evaluate( time ) * frameOverTime.curveMultiplier;
				else if ( frameOverTime.constant > 0 )
					frameProgress = frameOverTime.constant - ( particle.remainingLifetime / particle.startLifetime );

				frameProgress = Mathf.Repeat( frameProgress * _textureSheetAnimation.cycleCount, 1 );

				switch ( _textureSheetAnimation.animation )
				{
					case ParticleSystemAnimationType.WholeSheet:
						frame = Mathf.FloorToInt( frameProgress * _textureSheetAnimationFrames );
						break;

					case ParticleSystemAnimationType.SingleRow:
						frame = Mathf.FloorToInt( frameProgress * _textureSheetAnimation.numTilesX );

						int row = _textureSheetAnimation.rowIndex;
						//if (textureSheetAnimation.useRandomRow) { // FIXME - is this handled internally by rowIndex?
						//	row = Random.Range(0, textureSheetAnimation.numTilesY, using: particle.randomSeed);
						//}
						frame += row * _textureSheetAnimation.numTilesX;
						break;
				}

				frame %= _textureSheetAnimationFrames;

				particleUV.x = ( frame % _textureSheetAnimation.numTilesX ) * _textureSheetAnimationFrameSize.x;
				//TODO WTF? (1f-(y+size.y))
				particleUV.y = 1f -
				               ( Mathf.FloorToInt( frame / _textureSheetAnimation.numTilesX ) *
					               _textureSheetAnimationFrameSize.y + _textureSheetAnimationFrameSize.y );
				particleUV.z = particleUV.x + _textureSheetAnimationFrameSize.x;
				particleUV.w = particleUV.y + _textureSheetAnimationFrameSize.y;
			}

			temp.x = particleUV.x;
			temp.y = particleUV.y;

			_quad[ 0 ] = UIVertex.simpleVert;
			_quad[ 0 ].color = color;
			_quad[ 0 ].uv0 = temp;

			temp.x = particleUV.x;
			temp.y = particleUV.w;
			_quad[ 1 ] = UIVertex.simpleVert;
			_quad[ 1 ].color = color;
			_quad[ 1 ].uv0 = temp;

			temp.x = particleUV.z;
			temp.y = particleUV.w;
			_quad[ 2 ] = UIVertex.simpleVert;
			_quad[ 2 ].color = color;
			_quad[ 2 ].uv0 = temp;

			temp.x = particleUV.z;
			temp.y = particleUV.y;
			_quad[ 3 ] = UIVertex.simpleVert;
			_quad[ 3 ].color = color;
			_quad[ 3 ].uv0 = temp;

			if ( rotation == 0 )
			{
				// no rotation
				corner1.x = position.x - size;
				corner1.y = position.y - size;
				corner2.x = position.x + size;
				corner2.y = position.y + size;

				temp.x = corner1.x;
				temp.y = corner1.y;
				_quad[ 0 ].position = temp;
				temp.x = corner1.x;
				temp.y = corner2.y;
				_quad[ 1 ].position = temp;
				temp.x = corner2.x;
				temp.y = corner2.y;
				_quad[ 2 ].position = temp;
				temp.x = corner2.x;
				temp.y = corner1.y;
				_quad[ 3 ].position = temp;
			}
			else
			{
				// apply rotation
				Vector2 right = new Vector2( Mathf.Cos( rotation ), Mathf.Sin( rotation ) ) * size;
				Vector2 up = new Vector2( Mathf.Cos( rotation90 ), Mathf.Sin( rotation90 ) ) * size;

				_quad[ 0 ].position = position - right - up;
				_quad[ 1 ].position = position - right + up;
				_quad[ 2 ].position = position + right + up;
				_quad[ 3 ].position = position + right - up;
			}

			vh.AddUIVertexQuad( _quad );
		}
	}

	private void Update()
	{
		if ( !gameObject.activeInHierarchy || ( _canvasGroup != null && _canvasGroup.alpha <= 0f ) )
			return;

		if ( !fixedTime && Application.isPlaying )
		{
			if ( !Simulate() ) return;

			if ( ( _currentMaterial != null && _currentTexture != _currentMaterial.mainTexture ) ||
			     ( material != null && _currentMaterial != null && material.shader != _currentMaterial.shader ) )
			{
				_particleSystem = null;
				Initialize();
			}
		}
	}

	private void LateUpdate()
	{
		if ( !gameObject.activeInHierarchy || ( _canvasGroup != null && _canvasGroup.alpha <= 0f ) )
			return;

		if ( !Application.isPlaying )
		{
			SetAllDirty();
		}
		else
		{
			if ( fixedTime )
			{
				if ( !Simulate() ) return;

				if ( ( _currentMaterial != null && _currentTexture != _currentMaterial.mainTexture ) ||
				     ( material != null && _currentMaterial != null && material.shader != _currentMaterial.shader ) )
				{
					_particleSystem = null;
					Initialize();
					return;
				}
			}
		}

		if ( material != _currentMaterial )
		{
			_particleSystem = null;
			Initialize();
		}
	}

	private bool Simulate()
	{
		_delta += Time.unscaledDeltaTime;
		if ( _delta < _tick )
			return false;

		var deltaTime = _delta;
		_delta = 0f;

		_particleSystem.Simulate( deltaTime, false, false, true );
		SetAllDirty();

		if ( !CheckStopAction )
			return true;

		var duration = _particleSystem.main.duration;
		var elapsedTime = _particleSystem.time;

		if ( duration - elapsedTime < deltaTime )
		{
			switch ( _particleSystem.main.stopAction )
			{
				case ParticleSystemStopAction.None:
					break;
				case ParticleSystemStopAction.Disable:
					gameObject.SetActive( false );
					return false;
				case ParticleSystemStopAction.Destroy:
					Destroy( gameObject );
					return false;
				case ParticleSystemStopAction.Callback:
					throw new NotSupportedException();
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		return true;
	}

#if UNITY_EDITOR
	protected override void OnValidate()
	{
		base.OnValidate();

		var cacheTransform = transform;
		var cacheParticleSystem = GetComponent< ParticleSystem >();
		var changed = false;
		if ( _cacheTransform != cacheTransform )
		{
			_cacheTransform = cacheTransform;
			changed = true;
		}

		if ( _cacheParticleSystem != cacheParticleSystem )
		{
			_cacheParticleSystem = cacheParticleSystem;
			changed = true;
		}

		if ( changed )
		{
			UnityEditor.EditorUtility.SetDirty( gameObject );
		}
	}
#endif
}

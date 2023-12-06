Shader "UI/UI Particle"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

		[Header(Blending)]
		[Enum(UnityEngine.Rendering.BlendOp)] _BlendOp ("Operation", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendFactor ("Source", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlendFactor ("Destination", Float) = 10

		[Space(10)]
		[Header(Stencil Parameters)]
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Space(10)]
		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		BlendOp [_BlendOp]
		Blend [_SrcBlendFactor] [_DstBlendFactor]
		ColorMask [_ColorMask]

		Pass
		{
		CGPROGRAM
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile __ UNITY_UI_ALPHACLIP

			struct Input
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fixed4 _TextureSampleAdd;
			float4 _ClipRect;

			v2f vert(Input input)
			{
				v2f output;

				output.vertex = UnityObjectToClipPos(input.vertex);
				output.worldPosition = input.vertex;

				output.texcoord = TRANSFORM_TEX(input.texcoord, _MainTex);

			#ifdef UNITY_HALF_TEXEL_OFFSET
				output.vertex.xy += (_ScreenParams.zw - 1.0f) * float2(-1.0f, 1.0f);
			#endif

				output.color = input.color;

				return output;
			}

			fixed4 frag(v2f input) : SV_Target
			{
				fixed4 color = (tex2D(_MainTex, input.texcoord) + _TextureSampleAdd) * input.color;

				color.w *= UnityGet2DClipping(input.worldPosition.xy, _ClipRect);

			#ifdef UNITY_UI_ALPHACLIP
				clip(color.w - 0.001f);
			#endif

				return color;
			}
		ENDCG
		}
	}
}

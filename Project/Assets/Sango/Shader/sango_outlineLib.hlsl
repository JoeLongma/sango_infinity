#ifndef SANGO_OUTLINE_LIB
#define SANGO_OUTLINE_LIB

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
CBUFFER_START(UnityPerMaterial)
float4 _MainTex_ST;
float _OutlineWidth;

#if SANGO_WATER | SANGO_TERRAIN
half _Alpha;
#endif

#if SANGO_WATER
float _HorizontalAmount;
float _VerticalAmount;
float _Speed;
#endif

#if SANGO_BLEND_HEIGHT
float _BlendHeight;
float _BlendPower;
#endif

#if SANGO_COLOR
half4 _Color;
#endif

#if SANGO_BASE_COLOR || SANGO_BASE_COLOR_ADD
float _BaseColorIntensity;
#endif

CBUFFER_END

float _Power;
float _MixBegin;
float _MixPower;
float _MixEnd;

TEXTURE2D_X_FLOAT(_CameraDepthTexture);
SAMPLER(sampler_CameraDepthTexture);
TEXTURE2D(_MainTex);
#define smp SamplerState_Linear_Repeat
SAMPLER(smp);

struct VertexInput
{
	UNITY_VERTEX_INPUT_INSTANCE_ID
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float2 uv : TEXCOORD0;

};

struct VertexOutput
{
	UNITY_VERTEX_INPUT_INSTANCE_ID
	float4 pos : SV_POSITION;
	float4 screenPos : TEXCOORD2;
	float2 uv : TEXCOORD0;
};

VertexOutput outline_vert(VertexInput v)
{
	UNITY_SETUP_INSTANCE_ID(v);

	VertexOutput o;

	float camDist = distance(TransformObjectToWorld(v.vertex.xyz), _WorldSpaceCameraPos) * _OutlineWidth * 0.001;
	float3 _vertex = v.vertex.xyz;
	//v.vertex.xyz += normalize(v.vertex.xyz) * camDist ;
	v.vertex.xyz += normalize(v.normal.xyz) * camDist * 0.6;

	//v.vertex.xyz += normalize(v.vertex.xyz) * _OutlineWidth;
	o.uv = TRANSFORM_TEX(v.uv, _MainTex);

	o.pos = TransformObjectToHClip(v.vertex.xyz);
	o.screenPos = ComputeScreenPos(o.pos);
	UNITY_TRANSFER_INSTANCE_ID(v, o);

	return o;
}


half4 outline_frag(VertexOutput i) : SV_TARGET
{
	UNITY_SETUP_INSTANCE_ID(i);

	float2 screenPos = i.screenPos.xy / i.screenPos.w;
	float depth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, screenPos).r;
	float depthValue = Linear01Depth(depth, _ZBufferParams);
	float linear01Depth = pow(saturate((depthValue * 1500 - _MixBegin) / (_MixEnd - _MixBegin)), _MixPower);
	half4 _MainTex_var = SAMPLE_TEXTURE2D(_MainTex, smp, i.uv);

	clip(_MainTex_var.a - 0.5);

	half4 finalRGBA = half4(0,0,0, 0.6 * saturate(1 - linear01Depth));//saturate(1-linear01Depth));
	//UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
	return finalRGBA;
}

#endif
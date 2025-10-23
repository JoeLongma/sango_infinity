
Shader "Sango/building_urp" {
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
		_OutlineWidth("width", float) = 3.5
		_BaseColorIntensity("BaseColorFactor", float) = 1
		}
		SubShader{
			Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent" "RenderType" = "TransparentCutout" }
			LOD 300

			//Pass
			//{
			//	Name "OUTLINEPASS"
			//	Tags {
			//		"LightMode" = "SRPDefaultUnlit"
			//	}
			//	Fog { Mode Off }
			//	ZWrite On
			//	Cull Front
			//	Blend SrcAlpha OneMinusSrcAlpha
			//	HLSLPROGRAM
			//	#include "sango_outlineLib.hlsl"
			//	//#pragma multi_compile_fwdbase
			//	//#pragma multi_compile_fog
			//	#pragma skip_variants SHADOWS_SOFT DIRLIGHTMAP_COMBINED
			//	//#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
			//	#pragma multi_compile _ SANGO_EDITOR
			//	#pragma skip_variants FOG_EXP FOG_EXP2
			//	#pragma exclude_renderers xbox360 ps3 
			//	#pragma target 3.0
			//	#pragma vertex outline_vert
			//	#pragma fragment outline_frag
			//	ENDHLSL
			//}

			Pass {
				Name "FORWARD"
				Tags {
					"LightMode" = "UniversalForward"
				}
				Blend SrcAlpha OneMinusSrcAlpha
				HLSLPROGRAM
				#define SANGO_BASE_COLOR 1
				#define SANGO_FOG 1
				#define SANGO_ALPHA_TEST 1
				#define SANGO_TERRAIN_TYPE 1
				
				#include "sango_shaderLib.hlsl"
				//#pragma multi_compile_fwdbase
				//#pragma multi_compile_fog
				#pragma skip_variants SHADOWS_SOFT DIRLIGHTMAP_COMBINED
				//#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
				#pragma multi_compile _ SANGO_EDITOR
				#pragma skip_variants FOG_EXP FOG_EXP2
				#pragma exclude_renderers xbox360 ps3 
				#pragma target 3.0
				#pragma vertex sango_vert
				#pragma fragment sango_frag
				#pragma multi_compile_instancing

				ENDHLSL
			}


			pass {

					Name "ShadowCast"

					Tags{ "LightMode" = "ShadowCaster" }
					HLSLPROGRAM

					#pragma vertex ShadowPassVertex
					#pragma fragment ShadowPassFragment

					#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

					CBUFFER_START(UnityPerMaterial)
					CBUFFER_END

					struct Attributes
					{
						float4 positionOS   : POSITION;
						float3 normalOS     : NORMAL;
					};

					struct Varyings
					{
						float4 positionCS   : SV_POSITION;
					};

					Varyings ShadowPassVertex(Attributes input)
					{
						Varyings output;

						float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
						float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

						float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _MainLightPosition.xyz));
						output.positionCS = positionCS;
						return output;
					}
					half4 ShadowPassFragment(Varyings input) : SV_TARGET
					{
						return 0;
					}
					ENDHLSL
				}


			}

			//Fallback "Legacy Shaders/Diffuse"
}


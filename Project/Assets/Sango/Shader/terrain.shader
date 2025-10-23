
Shader "Sango/terrain_urp" {
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
		_BaseColorIntensity("BaseColorFactor", float) = 1
		_OutlineWidth("width", float) = 3.5
		_Alpha("Alpha", float) = 1

			//_ShadowColor("Shadow Color", Color) = (0.2,0.2,1,1)
			//_OutlineWidth("width", float) = 0.14//定义一个变量


			//_BaseTex("BaseTex", 2D) = "white" {}
			//_GridTex("Grid", 2D) = "white" {}
			//_GridMask("Grid Mask", 2D) = "black" {}
			//_GridFlag("gridFlag", float) = 1//定义一个变量
			//_DarkFlag("gridFlag", float) = 1//定义一个变量
			//_MixBegin("mixBegin", float) = 800//和天空盒混合距离
			//_MixEnd("mixEnd", float) = 800//和天空盒混合距离
			//_MixPower("mixPower", float) = 7.5//和天空盒混合强度

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
				Fog { Mode Off }
				Blend SrcAlpha OneMinusSrcAlpha
				HLSLPROGRAM
				#define SANGO_BASE_COLOR 1
				#define SANGO_GRID_COLOR 1
				#define SANGO_FOG 1
				#define SANGO_BRUSH 1
				#define SANGO_TERRAIN_TYPE 1
				#define SANGO_TERRAIN 1
				#include "sango_shaderLib.hlsl"

				#pragma skip_variants SHADOWS_SOFT DIRLIGHTMAP_COMBINED
				//#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
				#pragma multi_compile _ SANGO_EDITOR
				#pragma multi_compile_instancing

				#pragma skip_variants FOG_EXP FOG_EXP2
				#pragma exclude_renderers xbox360 ps3 
				#pragma target 3.0
				#pragma vertex sango_vert
				#pragma fragment sango_frag
				ENDHLSL
			}
		}

		Fallback "Sango/sango_urp"
}


Shader "UI/TextOutline"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1, 1, 1, 1)
            //_OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
            //_OutlineWidth ("Outline Width", Int) = 1

            _StencilComp("Stencil Comparison", Float) = 8
            _Stencil("Stencil ID", Float) = 0
            _StencilOp("Stencil Operation", Float) = 0
            _StencilWriteMask("Stencil Write Mask", Float) = 255
            _StencilReadMask("Stencil Read Mask", Float) = 255

            _ColorMask("Color Mask", Float) = 15

            [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
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
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }

            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask[_ColorMask]

            Pass
            {
                Name "OUTLINE"
                Tags { "LightMode" = "SRPDefaultUnlit" }
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

            //Add for RectMask2D  
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            //End for RectMask2D  

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _MainTex_TexelSize;
            //float4 _OutlineColor;
            //int _OutlineWidth;
            float4 _MainTex_ST;
            float _UIMaskSoftnessX;
            float _UIMaskSoftnessY;
            //Add for RectMask2D  
            float4 _ClipRect;
            //End for RectMask2D

            struct appdata
            {
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float4 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float2 uv3 : TEXCOORD3;
                fixed4 color : COLOR;
            };


            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 tangent : TANGENT;
                float4 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float2 uv3 : TEXCOORD3;
                //Add for RectMask2D  
                float4 worldPosition : TEXCOORD4;
                //End for RectMask2D
                float4  mask : TEXCOORD5;
                fixed4 color : COLOR;
            };

            v2f vert(appdata IN)
            {
                v2f o;

                //Add for RectMask2D  
                o.worldPosition = IN.vertex;
                //End for RectMask2D 

                o.vertex = UnityObjectToClipPos(IN.vertex);
                o.tangent = IN.tangent;
                o.texcoord = IN.texcoord;
                o.color = IN.color * _Color;
                o.uv1 = IN.uv1;
                o.uv2 = IN.uv2;
                o.uv3 = IN.uv3;
                o.normal = IN.normal;

                float2 pixelSize = o.vertex.w;
                pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

                float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                float2 maskUV = (IN.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);
                o.mask = float4(IN.vertex.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));


                return o;
            }
            /*
            fixed IsInRect(float2 pPos, float4 pClipRect)
            {
                pPos = step(pClipRect.xy, pPos) * step(pPos, pClipRect.zw);
                return pPos.x * pPos.y;
            }
            */
            fixed IsInRect(float2 pPos, float2 pClipRectMin, float2 pClipRectMax)
            {
                pPos = step(pClipRectMin, pPos) * step(pPos, pClipRectMax);
                return pPos.x * pPos.y;
            }

            fixed SampleAlpha(int pIndex, v2f IN)
            {
                const fixed sinArray[12] = { 0, 0.5, 0.866, 1, 0.866, 0.5, 0, -0.5, -0.866, -1, -0.866, -0.5 };
                const fixed cosArray[12] = { 1, 0.866, 0.5, 0, -0.5, -0.866, -1, -0.866, -0.5, 0, 0.5, 0.866 };
                float2 pos = IN.texcoord + _MainTex_TexelSize.xy * float2(cosArray[pIndex], sinArray[pIndex]) * IN.normal.z;	//normal.z 存放 _OutlineWidth
                return IsInRect(pos, IN.uv1, IN.uv2) * (tex2D(_MainTex, pos) + _TextureSampleAdd).w * IN.tangent.w;		//tangent.w 存放 _OutlineColor.w
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
            //if (IN.normal.z > 0)	//normal.z 存放 _OutlineWidth
            //{
                color.w *= IsInRect(IN.texcoord, IN.uv1, IN.uv2);	//uv1 uv2 存着原始字的uv长方形区域大小
                half3 c = half3(IN.uv3.x, IN.uv3.y, IN.tangent.z);

                //c = pow(c, 2.2);
                half4 val = half4(c, 0);		//uv3.xy tangent.z 分别存放着 _OutlineColor的rgb
                val.w += SampleAlpha(0, IN);
                val.w += SampleAlpha(1, IN);
                val.w += SampleAlpha(2, IN);
                val.w += SampleAlpha(3, IN);
                val.w += SampleAlpha(4, IN);
                val.w += SampleAlpha(5, IN);
                val.w += SampleAlpha(6, IN);
                val.w += SampleAlpha(7, IN);
                val.w += SampleAlpha(8, IN);
                val.w += SampleAlpha(9, IN);
                val.w += SampleAlpha(10, IN);
                val.w += SampleAlpha(11, IN);
                val.w = saturate(val.w);
                color = (val * (1.0 - color.a)) + (color * color.a);
                color = val;
                //color.a *= IN.color.a ;	//字逐渐隐藏时，描边也要隐藏

                //}

                //Add for RectMask2D 
                    half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
                   // color.a *= m.x * m.y;

#ifdef UNITY_UI_ALPHACLIP
                //clip(color.a - 0.001);
#endif
                //End for RectMask2D 

                float g = IN.normal.x; // 1 if  gray, or 0
                //if (g == 1){
                fixed3 gray = dot(color.rgb, float3(0.299, 0.587, 0.114));
                color.rgb = lerp(color.rgb, gray, g);
                //}
                return color;
            }

            ENDCG
        }

         Pass
        {
            Name "Base"
            Tags { "LightMode" = "UniversalForward" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

                //Add for RectMask2D  
                  #include "UnityCG.cginc"
                #include "UnityUI.cginc"
                //End for RectMask2D  

                sampler2D _MainTex;
                fixed4 _Color;
                fixed4 _TextureSampleAdd;
                float4 _MainTex_TexelSize;

                //float4 _OutlineColor;
                //int _OutlineWidth;
                float4 _MainTex_ST;
                float _UIMaskSoftnessX;
                float _UIMaskSoftnessY;
                //Add for RectMask2D  
                float4 _ClipRect;
                //End for RectMask2D

                struct appdata
                {
                    float4 vertex : POSITION;
                    float4 tangent : TANGENT;
                    float4 normal : NORMAL;
                    float2 texcoord : TEXCOORD0;
                    float2 uv1 : TEXCOORD1;
                    float2 uv2 : TEXCOORD2;
                    float2 uv3 : TEXCOORD3;
                    fixed4 color : COLOR;
                };


                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float4 tangent : TANGENT;
                    float4 normal : NORMAL;
                    float2 texcoord : TEXCOORD0;
                    float2 uv1 : TEXCOORD1;
                    float2 uv2 : TEXCOORD2;
                    float2 uv3 : TEXCOORD3;
                    //Add for RectMask2D  
                    float4 worldPosition : TEXCOORD4;
                    float4  mask : TEXCOORD5;
                    //End for RectMask2D
                    fixed4 color : COLOR;
                };

                v2f vert(appdata IN)
                {
                    v2f o;

                    //Add for RectMask2D  
                    o.worldPosition = IN.vertex;
                    //End for RectMask2D 

                    o.vertex = UnityObjectToClipPos(IN.vertex);
                    o.tangent = IN.tangent;
                    o.texcoord = IN.texcoord;
                    o.color = IN.color * _Color;
                    o.uv1 = IN.uv1;
                    o.uv2 = IN.uv2;
                    o.uv3 = IN.uv3;
                    o.normal = IN.normal;
                    float2 pixelSize = o.vertex.w;
                    pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

                    float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                    float2 maskUV = (IN.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);
                    o.mask = float4(IN.vertex.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));

                    return o;
                }
                /*
                fixed IsInRect(float2 pPos, float4 pClipRect)
                {
                    pPos = step(pClipRect.xy, pPos) * step(pPos, pClipRect.zw);
                    return pPos.x * pPos.y;
                }
                */
                fixed IsInRect(float2 pPos, float2 pClipRectMin, float2 pClipRectMax)
                {
                    pPos = step(pClipRectMin, pPos) * step(pPos, pClipRectMax);
                    return pPos.x * pPos.y;
                }

                fixed SampleAlpha(int pIndex, v2f IN)
                {
                    const fixed sinArray[12] = { 0, 0.5, 0.866, 1, 0.866, 0.5, 0, -0.5, -0.866, -1, -0.866, -0.5 };
                    const fixed cosArray[12] = { 1, 0.866, 0.5, 0, -0.5, -0.866, -1, -0.866, -0.5, 0, 0.5, 0.866 };
                    float2 pos = IN.texcoord + _MainTex_TexelSize.xy * float2(cosArray[pIndex], sinArray[pIndex]) * IN.normal.z;	//normal.z 存放 _OutlineWidth
                    return IsInRect(pos, IN.uv1, IN.uv2) * (tex2D(_MainTex, pos) + _TextureSampleAdd).w * IN.tangent.w;		//tangent.w 存放 _OutlineColor.w
                }

                fixed4 frag(v2f IN) : SV_Target
                {
                    fixed4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
                    color.w *= IsInRect(IN.texcoord, IN.uv1, IN.uv2);
                    ////if (IN.normal.z > 0)	//normal.z 存放 _OutlineWidth
                    ////{
                    //	color.w *= IsInRect(IN.texcoord, IN.uv1, IN.uv2);	//uv1 uv2 存着原始字的uv长方形区域大小
                    //	half3 c =half3(IN.uv3.x, IN.uv3.y, IN.tangent.z);
        //                c= pow(c, 2.2);
        //                half4 val = half4(c, 0);		//uv3.xy tangent.z 分别存放着 _OutlineColor的rgb
                    //	val.w += SampleAlpha(0, IN);
                    //	val.w += SampleAlpha(1, IN);    
                    //	val.w += SampleAlpha(2, IN);
                    //	val.w += SampleAlpha(3, IN);
                    //	val.w += SampleAlpha(4, IN);
                    //	val.w += SampleAlpha(5, IN);
                    //	val.w += SampleAlpha(6, IN);
                    //	val.w += SampleAlpha(7, IN);
                    //	val.w += SampleAlpha(8, IN);
                    //	val.w += SampleAlpha(9, IN);
                    //	val.w += SampleAlpha(10, IN);
                    //	val.w += SampleAlpha(11, IN);

                    //	color = (val * (1.0 - color.a)) + (color * color.a);

                        color.a *= IN.color.a;	//字逐渐隐藏时，描边也要隐藏

                        //}

                        //Add for RectMask2D 
                            half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
                            color.a *= m.x * m.y;
        #ifdef UNITY_UI_ALPHACLIP
                        clip(color.a - 0.001);
        #endif
                        //End for RectMask2D 

                        float g = IN.normal.x; // 1 if  gray, or 0
                        //if (g == 1){
                        fixed3 gray = dot(color.rgb, float3(0.299, 0.587, 0.114));
                        color.rgb = lerp(color.rgb, gray, g);
                        //}

                        //color.a = clamp(color.a,0,1);
                        return color;
                    }

                    ENDCG
                }
        }
}
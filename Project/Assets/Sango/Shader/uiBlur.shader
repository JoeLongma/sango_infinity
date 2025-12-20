Shader "Sango/uiBlur"
{
    Properties
    {
        _BaseMap("MainTex",2D) = "white"{}
        _Color("Color",Color) = (1,1,1,1)
        _Radius("半径",Range(0,10)) = 0
    }

        SubShader
        {
            Pass
            {
                Blend SrcAlpha OneMinusSrcAlpha

                CGPROGRAM

                #pragma vertex vert
                #pragma fragment frag 

                fixed4 _Color;
                sampler2D _MainTex;
                float4 _MainTex_TexelSize;

                float _Radius;
                struct a2v
                {
                    float4 vertex : POSITION;
                    fixed4 color : COLOR;
                    float2 uv     : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos   : SV_POSITION;
                    fixed4 color : COLOR;
                    float2 uv    : TEXCOORD0;
                };

                fixed4 Blur(sampler2D tex, half2 uv, half2 blurSize)
                {
                    int KERNEL_SIZE = 3;

                    float4 o = 0;
                    float sum = 0;
                    float weight;
                    half2 texcood;
                    for (int x = -KERNEL_SIZE / 2; x <= KERNEL_SIZE / 2; x++)
                    {
                        for (int y = -KERNEL_SIZE / 2; y <= KERNEL_SIZE / 2; y++)
                        {
                            texcood = uv;
                            texcood.x += blurSize.x * x;
                            texcood.y += blurSize.y * y;
                            weight = 1.0 / (abs(x) + abs(y) + 2);
                            o += tex2D(tex, texcood) * weight;
                            sum += weight;
                        }
                    }
                    return o / sum;
                }

                v2f vert(a2v v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.color = v.color;
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_TARGET
                {
                    fixed4 col = Blur(_MainTex,i.uv,_Radius * _MainTex_TexelSize.xy);
                    return col;
                }

                ENDCG
            }
        }
}

#ifndef Troops_UNIVERSAL_SHADOW_CASTER_PASS_INCLUDED
#define Troops_UNIVERSAL_SHADOW_CASTER_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

// Shadow Casting Light geometric parameters. These variables are used when applying the shadow Normal Bias and are set by UnityEngine.Rendering.Universal.ShadowUtils.SetupShadowCasterConstantBuffer in com.unity.render-pipelines.universal/Runtime/ShadowUtils.cs
// For Directional lights, _LightDirection is used when applying shadow Normal Bias.
// For Spot lights and Point lights, _LightPosition is used to compute the actual light direction because it is different at each shadow caster geometry vertex.
float3 _LightDirection;
float3 _LightPosition;

struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float2 texcoord     : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv           : TEXCOORD0;
    float4 positionCS   : SV_POSITION;
};

//定义InstancingBuffer的结构体
UNITY_INSTANCING_BUFFER_START(troops)
UNITY_DEFINE_INSTANCED_PROP(float, _StartTime)
UNITY_INSTANCING_BUFFER_END(troops)

float4 GetShadowPositionHClip(Attributes input)
{
    float3 center = float3(0, 0, 0);
    //物体空间原点
   //将相机位置转换至物体空间并计算相对原点朝向，物体旋转后的法向将与之平行，这里实现的是Viewpoint-oriented Billboard
    //float3 viewer = mul(unity_WorldToObject,float4(_WorldSpaceCameraPos, 1));

    float3 viewer = TransformWorldToObject(_WorldSpaceCameraPos.xyz);

    float3 normalDir = viewer - center;
    // _VerticalBillboarding为0到1，控制物体法线朝向向上的限制，实现Axial Billboard到World-Oriented Billboard的变换
    //normalDir.y =	normalDir.y * _VerticalBillboarding;
    normalDir.y = 0;
    normalDir = normalize(normalDir);
    //若原物体法线已经朝向上，这up为z轴正方向，否者默认为y轴正方向
    float3 upDir = abs(normalDir.y) > 0.999 ? float3(0, 0, 1) : float3(0, 1, 0);
    //利用初步的upDir计算righDir，并以此重建准确的upDir，达到新建转向后坐标系的目的
    float3 rightDir = normalize(cross(upDir, normalDir));     upDir = normalize(cross(normalDir, rightDir));
    // 计算原物体各顶点相对位移，并加到新的坐标系上
    float3 centerOffs = input.positionOS.xyz - center;
    float3 localPos = center + rightDir * centerOffs.x + upDir * centerOffs.y;


    float3 positionWS = TransformObjectToWorld(localPos);
    float3 normalWS = TransformObjectToWorldNormal(normalDir);

#if _CASTING_PUNCTUAL_LIGHT_SHADOW
    float3 lightDirectionWS = normalize(_LightPosition - positionWS);
#else
    float3 lightDirectionWS = _LightDirection;
#endif

    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

#if UNITY_REVERSED_Z
    positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#else
    positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#endif

    return positionCS;
}

Varyings ShadowPassVertex(Attributes input)
{
    Varyings output;
    UNITY_SETUP_INSTANCE_ID(input);

    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
    output.positionCS = GetShadowPositionHClip(input);
    return output;
}

half4 ShadowPassFragment(Varyings input) : SV_TARGET
{
    float iStartTime = UNITY_ACCESS_INSTANCED_PROP(troops, _StartTime);


    float time = floor(_Speed * _Time.y + iStartTime);
    //time = time - floor(time / 18) * 18;  

    float row = _VerticalIndex;// floor(time / _HorizontalAmount);    // /运算获取当前行

    float column = floor(time % _HorizontalMax);  // %运算获取当前列

    //首先把原纹理坐标i.uv按行数和列数进行等分，然后使用当前的行列进行偏移
    half2 uv = input.uv + half2(column, row);
    uv.x /= _HorizontalAmount;
    uv.y /= _VerticalAmount;

    half4 _MainTex_var = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);

    Alpha(_MainTex_var.a, _BaseColor, _Cutoff);
    return 0;
}

#endif

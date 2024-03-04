Shader "ShaderMagic/Transition"
{
    Properties
    {
        _start_time ("Start Time", Float) = 0.0
        _duration ("Duration", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        ZWrite Off Cull Off
        Pass
        {
            Name "TransitionPass"
            
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "../Core/color.hlsl"
            
            #pragma vertex Vert
            #pragma fragment Frag

            float _start_time = 0.1;
            float _duration = 10.;
            
            float4 Frag(Varyings input) : SV_Target0
            {
               return float4(0, 0, 0, 1);
            }
            ENDHLSL
        }
    }
}

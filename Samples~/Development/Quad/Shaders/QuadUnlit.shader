Shader "ShaderMagic/QuadUnlit"
{
    Properties
    {
        _ColorA("Color A", Color) = (1,0,0,1)
        _ColorB("Color B", Color) = (0,0,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment TwoColorBlendFragment
            
            #include "UnityCG.cginc"
            #include "QuadUnlitCG.cginc"
            ENDCG
        }
    }
}

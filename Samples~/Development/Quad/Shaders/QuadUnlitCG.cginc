fixed4 _ColorA;
fixed4 _ColorB;

struct vert2frag // vertext to fragment
{
    float4 vertex : SV_POSITION; // system value position
    float4 position : TEXCOORD1;
    float2 uv : TEXCOORD0;
};

vert2frag Vert(appdata_base v)
{
    vert2frag o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.position = v.vertex;
    o.uv = v.texcoord;
    return o;
}

fixed4 SingleColorFragment(v2f_img i) : SV_Target
{
    return _ColorA.grba;
}

fixed4 ColorGradientFragment(v2f_img i) : SV_Target
{
    fixed3 color = fixed3((sin(_Time.w) + 1) / 2, 0, (cos(_Time.w) + 1 / 2));
    return fixed4(color, 1).gbra;
}

fixed4 TwoColorGradientFragment(v2f_img i) : SV_Target
{
    float delta = (sin(_Time.y) + 1) / 2;
    fixed3 color = lerp(_ColorA, _ColorB, delta);
    return fixed4(color, 1);
}

fixed4 TwoColorBlendFragment(v2f_img i) : SV_Target
{
    float delta = i.uv.x;
    fixed3 color = lerp(_ColorA, _ColorB, delta);
    return fixed4(color, 1.0);
}

fixed4 Saturate(vert2frag i) : SV_Target
{
    fixed3 color = saturate(i.position * 2);
    return fixed4(color, 1.0);
}

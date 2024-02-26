#ifndef SHADERMAGIC_COLOR_EFFECT
#define SHADERMAGIC_COLOR_EFFECT

void color_ripple_float(float time, float start_time, float3 position, float3 center, float ripple_speed,
    float smooth, float3 ripple_color, float3 base_color, out float3 color)
{
    float ripple = distance(position, center) / (ripple_speed * (time - start_time));
    color = lerp(ripple_color, base_color, smoothstep(smooth, 1., ripple));
}

#endif
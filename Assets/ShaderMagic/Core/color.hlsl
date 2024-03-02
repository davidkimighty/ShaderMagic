#ifndef SHADERMAGIC_COLOR
#define SHADERMAGIC_COLOR

void get_solar_spectrum_float(float wavelength, out float3 color)
{
    float3 v = float3(128, 43,  255)/255;
    float3 b = float3(0,   0,   255)/255;
    float3 c = float3(0,   255, 255)/255;
    float3 g = float3(0,   255, 0)/255;
    float3 y = float3(255, 255, 0)/255;
    float3 o = float3(255, 165, 0)/255;
    float3 r = float3(255, 0,   0)/255;
    float3 black = 0;

    color = lerp(
        lerp(
            lerp(v, b, smoothstep(380, 470, wavelength)),
            lerp(c, g, smoothstep(560, 630, wavelength)),
            smoothstep(430, 590, wavelength)
        ),
        lerp(
            lerp(y, o, smoothstep(600, 680, wavelength)),
            r,
            smoothstep(650, 720, wavelength)
        ),
        smoothstep(510, 610, wavelength)
    );
    
    color = lerp(
        lerp(black, color, smoothstep(380, 500, wavelength)),
        lerp(color, black, smoothstep(690, 750, wavelength)),
        smoothstep(380, 750, wavelength)
    );
}

#endif
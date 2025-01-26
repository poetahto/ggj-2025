float random (in float2 st) {
    return frac(sin(dot(st.xy,
        float2(12.9898,78.233)))*
        43758.5453123);
}

void random_float (in float2 In, out float Out) {
    Out = frac(sin(dot(In.xy,
        float2(12.9898,78.233)))*
        43758.5453123);
}

// Based on Morgan McGuire @morgan3d
// https://www.shadertoy.com/view/4dS3Wd
void noise_float(in float2 In, out float Out) {
    float2 i = floor(In);
    float2 f = frac(In);

    // Four corners in 2D of a tile
    float a = random(i);
    float b = random(i + float2(0.110,0.820));
    float c = random(i + float2(0.0, 1.0));
    float d = random(i + float2(1.0, 1.0));

    float2 u = f * f * (3.0 - 2.0 * f);
    Out = lerp(a, b, u.x) +
            (c - a) * u.y * (1.0 - u.x) +
            (d - b) * u.x * u.y;
}

float noise(in float2 st) {
    float2 i = floor(st);
    float2 f = frac(st);

    // Four corners in 2D of a tile
    float a = random(i);
    float b = random(i + float2(0.110,0.820));
    float c = random(i + float2(0.0, 1.0));
    float d = random(i + float2(1.0, 1.0));

    float2 u = f * f * (3.0 - 2.0 * f);

    return lerp(a, b, u.x) +
            (c - a)* u.y * (1.0 - u.x) +
            (d - b) * u.x * u.y;
}

void simple_noise_float(in float2 In, out float Out){
    float2 i = floor(In);
    float2 f = frac(In);

    // Four corners in 2D of a tile
    float a = random(i);
    float b = random(i + float2(0.110,0.820));
    float c = random(i + float2(0.0, 1.0));
    float d = random(i + float2(1.0, 1.0));

    float2 u = f * f * (3.0 - 2.0 * f);
    Out = d;
}

void fbm_float(in float2 In, in float Octaves, out float Out) {
    // Initial values
    float value = 0.0;
    float amplitude = .5;
    float frequency = 0.;
    //
    // Loop of octaves
    for (int i = 0; i < Octaves; i++) {
        value += amplitude * noise(In);
        In *= 2.;
        amplitude *= .5;
    }
    Out = value;
}
shader_type canvas_item;

// Uniforms
uniform float wave_intensity : hint_range(0.0, 1.0) = 0.1;
uniform vec4 color_start : source_color = vec4(1.0, 0.5, 0.0, 1.0);
uniform vec4 color_end : source_color = vec4(1.0, 0.0, 0.5, 1.0);

void fragment() {
    // Get UV coordinates
    vec2 uv = UV;

    // Apply wave distortion
    uv.x += sin(uv.y * 10.0 + TIME) * wave_intensity;

    // Create vertical gradient
    vec4 gradient_color = mix(color_start, color_end, uv.y);

    // Output final color
    COLOR = gradient_color;
}

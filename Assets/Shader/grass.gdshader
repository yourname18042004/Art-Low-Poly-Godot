shader_type spatial;
render_mode cull_disabled;

varying vec3 world_pos;
uniform vec3 color : source_color;
uniform vec3 color2 : source_color;
uniform sampler2D noise;
uniform float noiseScale = 20.0;

void vertex() {
	world_pos = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
	vec3 noiseLevel = texture(noise, world_pos.xz / noiseScale).rgb;
	if(VERTEX.y > 0.0){
		VERTEX.x += 0.2 * VERTEX.y * sin(TIME + noiseLevel.r * PI);	
	}
}

void fragment() {
	ALBEDO = mix(color, color2, UV.y);
	if(!FRONT_FACING){
		NORMAL = -NORMAL;
	}
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}

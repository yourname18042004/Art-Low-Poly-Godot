shader_type spatial;

uniform sampler2D texture_height;
varying vec3 world_position;
varying float height;

uniform float size;
uniform float height_scale;

uniform vec3 albedo : source_color;
uniform vec3 color : source_color;
uniform vec3 color2 : source_color;

void vertex() {
	world_position = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
}

uniform float n = 50.0;
uniform float n2 = 30.0;
uniform float n3 = 10.0;

void fragment() {
	float r = length(world_position.xz);

	if(r <= n2)
		ALBEDO =  color;
	else if(r < n && r > n2)
		ALBEDO = mix(color, albedo, (r - n2)/(n - n2));
	else
		ALBEDO = albedo;
	//vec3 normal = texture(texture_height, world_position.xz/size).rgb;
	//NORMAL_MAP = normal;
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}

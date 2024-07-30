shader_type spatial;

uniform vec3 color : source_color;

void vertex() {
	
	VERTEX.y += 1.0 * sin(TIME * 0.5);
	
}

void fragment() {
	ALBEDO = color;
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}

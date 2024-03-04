shader_type canvas_item;

uniform bool active = false;

void fragment() {
	vec4 prevColor = texture(TEXTURE, UV);
	vec4 whiteColor = vec4(1.0, 1.0, 1.0, prevColor.a);
	vec4 newColor = prevColor;
	if(active) {
		newColor = (prevColor + whiteColor) / vec4(2, 2, 2, 1);
	}
	COLOR = newColor;
}

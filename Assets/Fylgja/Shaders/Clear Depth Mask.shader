// Upgrade NOTE: replaced 'PositionFog()' with multiply of UNITY_MATRIX_MVP by position

Shader "Fylgja/Clear Depth Mask" {
	SubShader {
		Tags{"Queue" = "Geometry+5"}
		Colormask A

		Zwrite on
		Pass{

			}
	} 
	FallBack "Diffuse", 1
}

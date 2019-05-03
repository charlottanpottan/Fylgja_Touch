Shader "Fylgja/Depth Mask" {
	SubShader {
		Tags{"Queue" = "Geometry+5"}
		Colormask A
		ztest Lequal
		Zwrite on
		Pass{
			
			}
	} 
	FallBack "Diffuse", 1
}

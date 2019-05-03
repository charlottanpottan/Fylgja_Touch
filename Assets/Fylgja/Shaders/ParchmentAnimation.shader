Shader "Fylgja/ParchmentAnimation"
{
	Properties 
	{
_MainTex("_MainTex", 2D) = "white" {}
_AnimationPosition("_AnimationPosition", Float) = 0
_FrameDivision("_FrameDivision", Float) = 0.5
_Color("_Color", Color) = (1,1,1,1)

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Geometry+0"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}

		
Cull Back
ZWrite On
ZTest LEqual


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 2.0


sampler2D _MainTex;
float _AnimationPosition;
float _FrameDivision;
float4 _Color;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				viewDir = normalize(viewDir);
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot (s.Normal, lightDir));
				
				float nh = max (0, dot (s.Normal, h));
				float3 spec = pow (nh, s.Specular*128.0) * s.Gloss;
				
				half4 res;
				res.rgb = _LightColor0.rgb * (diff * atten * 2.0);
				res.w = spec * Luminance (_LightColor0.rgb);

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float2 uv_MainTex;

			};


			void vert (inout appdata_full v, out Input o) {
                         UNITY_INITIALIZE_OUTPUT(Input,o);
float4 Vertex_VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 Vertex_VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 Vertex_VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 Vertex_VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Albedo = 0.0;
				o.Normal = float3(0.0,0.0,1.0);
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Alpha = 1.0;
float4 Pixel_Floor0=floor(float4(_AnimationPosition,_AnimationPosition,_AnimationPosition,_AnimationPosition));
float4 Pixel_Multiply0=Pixel_Floor0 * float4(_FrameDivision,_FrameDivision,_FrameDivision,_FrameDivision);
float4 Pixel_UV_Pan0=float4((IN.uv_MainTex.xyxy).x + Pixel_Multiply0.x,(IN.uv_MainTex.xyxy).y,(IN.uv_MainTex.xyxy).z,(IN.uv_MainTex.xyxy).w);
float4 Pixel_Tex2D0=tex2D(_MainTex,Pixel_UV_Pan0.xy);
float4 Pixel_Multiply1=Pixel_Tex2D0 * _Color;
float4 Pixel_Master0_1_NoInput = float4(0,0,1,1);
float4 Pixel_Master0_2_NoInput = float4(0,0,0,0);
float4 Pixel_Master0_3_NoInput = float4(0,0,0,0);
float4 Pixel_Master0_4_NoInput = float4(0,0,0,0);
float4 Pixel_Master0_5_NoInput = float4(1,1,1,1);
float4 Pixel_Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Pixel_Multiply1;
o.Alpha = 1.0;

			}
		ENDCG
	}
	Fallback "Diffuse"
}
Shader "Fylgja/MenuButton"
{
	Properties 
	{
_Color("_Color", Color) = (1,1,1,1)
_MainTex("_MainTex", 2D) = "black" {}
_BlendTex("_BlendTex", 2D) = "gray" {}
_BlendRange("_BlendRange", Range(0,1) ) = 0
_Cutoff("_Cutoff", Range(0,1) ) = 0

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Overlay"
"IgnoreProjector"="False"
"RenderType"="Overlay"

		}

		
Cull Back
ZWrite Off
ZTest LEqual
ColorMask RGBA
Blend SrcAlpha OneMinusSrcAlpha
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 2.0


float4 _Color;
sampler2D _MainTex;
sampler2D _BlendTex;
float _BlendRange;
float _Cutoff;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float2 uv_MainTex;
float2 uv_BlendTex;

			};

			void vert (inout appdata_full v, out Input o) {
             UNITY_INITIALIZE_OUTPUT(Input,o);
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Tex2D1=tex2D(_MainTex,(IN.uv_MainTex.xyxy).xy);
float4 Tex2D0=tex2D(_BlendTex,(IN.uv_BlendTex.xyxy).xy);
float4 Lerp0=lerp(Tex2D1,Tex2D0,_BlendRange.xxxx);
float4 Multiply1=_Color * Lerp0;
float4 Lerp1=lerp(Tex2D1.aaaa,Tex2D0.aaaa,_BlendRange.xxxx);
float4 Subtract0=Lerp1 - _Cutoff.xxxx;
float4 Master0_0_NoInput = float4(0,0,0,0);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Emission = Multiply1;
o.Alpha = Subtract0;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}
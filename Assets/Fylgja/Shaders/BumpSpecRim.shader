Shader "Fylgja/BumpedSpecularwRim"
{
	Properties 
	{
_Color("_Color", Color) = (1,0,0,1)
_MainTex("_MainTex", 2D) = "white" {}
_Bump("_Bump", 2D) = "white" {}
_RimPower("_RimPower", Range(0.1,3) ) = 1.8
_RimColor("_RimColor", Color) = (0.0597015,0.2503912,1,1)
_SpecPower("_SpecPower", Range(0.1,1) ) = 0.9
_MySpecColor("_MySpecColor", Color) = (1,1,1,1)

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Geometry"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 2.0


float4 _Color;
sampler2D _MainTex;
sampler2D _Bump;
float _RimPower;
float4 _RimColor;
float _SpecPower;
float4 _MySpecColor;

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
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot (s.Normal, lightDir));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff * atten * 2.0;
				res.w = spec * Luminance (_LightColor0.rgb);

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float2 uv_MainTex;
float2 uv_Bump;
float3 viewDir;

			};


			void vert (inout appdata_full v, out Input o) {
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
				o.Gloss=0.0;
				o.Specular=0.0;
				
float4 Tex2D0=tex2D(_MainTex,(IN.uv_MainTex.xyxy).xy);
float4 Multiply1=_Color * Tex2D0;
float4 Tex2DNormal0=float4(UnpackNormal( tex2D(_Bump,(IN.uv_Bump.xyxy).xy)).xyz, 1.0 );
float4 Fresnel0_1_NoInput = float4(0,0,1,1);
float4 Fresnel0=float4( 1.0 - dot( normalize( float4(IN.viewDir, 1.0).xyz), normalize( Fresnel0_1_NoInput.xyz ) ) );
float4 Pow0=pow(Fresnel0,_RimPower.xxxx);
float4 Multiply3=Tex2D0 * Pow0;
float4 Multiply0=Multiply3 * _RimColor;
float4 Multiply2=float4( Tex2D0.a) * _MySpecColor;
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Multiply1;
o.Normal = Tex2DNormal0;
o.Emission = Multiply0;
o.Specular = _SpecPower.xxxx;
o.Gloss = Multiply2;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}
Shader "Fylgja/BlacksmithSwordShader"
{
	Properties 
	{
_MainTex("_MainTex", 2D) = "white" {}
_Color("_Color", Color) = (1,1,1,1)
_AlbedoIntoEmissive("_AlbedoIntoEmissive", Float) = 0.2
_RimColor_01("_RimColor_01", Color) = (1,1,1,1)
_RimColor_02("_RimColor_02", Color) = (1,0.4823529,0,1)
_RimPower("_RimPower", Float) = 2.5
_RimStrength("_RimStrength", Float) = 10
_Emissive("_Emissive", Range(0,1) ) = 0

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


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 2.0


sampler2D _MainTex;
float4 _Color;
float _AlbedoIntoEmissive;
float4 _RimColor_01;
float4 _RimColor_02;
float _RimPower;
float _RimStrength;
float _Emissive;

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
c.a = s.Alpha + Luminance(spec);
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
float3 viewDir;

			};


			void vert (inout appdata_full v, out Input o) {
                         UNITY_INITIALIZE_OUTPUT(Input,o);
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Albedo = 0.0;
				o.Normal = float3(0.0,0.0,1.0);
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Alpha = 1.0;
float4 Tex2D0=tex2D(_MainTex,(IN.uv_MainTex.xyxy).xy);
float4 Multiply2=Tex2D0 * _Color;
float4 Multiply3=Tex2D0 * float4(_AlbedoIntoEmissive,_AlbedoIntoEmissive,_AlbedoIntoEmissive,_AlbedoIntoEmissive);
float4 Fresnel0_1_NoInput = float4(0,0,1,1);
float val = 1.0 - dot( normalize( float4(IN.viewDir, 1.0).xyz), normalize( Fresnel0_1_NoInput.xyz ) );
float4 Fresnel0=float4( val,val,val,val );
float4 Pow0=pow(Fresnel0,float4(_RimPower,_RimPower,_RimPower,_RimPower));
float4 Multiply1=Pow0 * float4(_RimStrength,_RimStrength,_RimStrength,_RimStrength);
float4 Lerp0=lerp(_RimColor_01,_RimColor_02,Multiply1);
float4 Add0=Multiply3 + Lerp0;
float4 Multiply0=Add0 * _Emissive.xxxx;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Multiply2;
o.Emission = Multiply0;
o.Alpha = 1.0;

			}
		ENDCG
	}
	Fallback "Diffuse"
}
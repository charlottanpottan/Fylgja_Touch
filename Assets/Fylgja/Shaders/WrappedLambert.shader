Shader "Fylgja/Wrapped Lambert" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
        _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf WrapLambert
  half4 LightingWrapLambert (SurfaceOutput s, half3 dir, half atten) {
      dir = normalize(dir);
      half NdotL = dot (s.Normal, dir);
      half diff = NdotL * 0.5 + 0.5;
      half4 c;
      c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);
      c.a = s.Alpha;
      return c;
  }
  struct Input {
      float2 uv_MainTex;
      float2 uv_BumpMap;
      float3 viewDir;
  };
  sampler2D _MainTex;
  sampler2D _BumpMap;
  float4 _RimColor;
  float _RimPower;
  void surf (Input IN, inout SurfaceOutput o) {
      o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
      o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
      half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          o.Emission = _RimColor.rgb * pow (rim, _RimPower);
  }
		ENDCG
	} 
	FallBack "Diffuse"
}

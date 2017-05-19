Shader "AxeyWorks/Mobile Distortion" {
	Properties{
		_Note("Use mob. dist. script!", float) = 0
		_MainTex("Base (RGB) Alpha (A)", 2D) = "white" {}
		_Color("Main Colour", Color) = (1,1,1,1)
		_WaveAndDistance("Distortion Distance", Vector) = (3.6, 12, 1, 1)
		_Cutoff("Cutoff", float) = 0.5
	}



		SubShader
	{
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }

		Lighting Off

		CGPROGRAM
		#pragma surface surf Lambert vertex:AtsWavingGrassVert addshadow
		#include "TerrainEngine.cginc"
		#include "AxeyWorksMobDist.cginc"



	struct Input
	{
		float2 uv_MainTex;
		float2 uv_Color;
	};
	sampler2D _MainTex;
	fixed4 _Color;
	fixed _Cutoff;


	void surf(Input IN, inout SurfaceOutput o) {
		clip(tex2D(_MainTex, IN.uv_MainTex).a - _Cutoff);
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Emission = 0;
		o.Alpha = c.a;
	}
	ENDCG
	}

		Fallback "Unlit/Transparent Cutout"
}
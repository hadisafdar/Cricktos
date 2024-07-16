Shader "StickSports/StickCricket/Pitch_Cheap" {
	Properties {
		_MainTex ("Pitch Tex", 2D) = "black" {}
		_GrassColour ("Grass Colour", Vector) = (0.5019608,0.5019608,0.5019608,1)
		_PitchColour ("Grass Colour", Vector) = (0.5019608,0.5019608,0.5019608,1)
		_LineColour ("Grass Colour", Vector) = (0.5019608,0.5019608,0.5019608,1)
		_Intensity ("Colour Intensity", Float) = 1
		_GrassScale ("Grass Scale", Float) = 40
		_GradientScale ("Gradient Scale", Float) = 0.6
		_GradientOffset ("Gradient Offset", Float) = 0.2
		_PatternScale ("Pattern Scale", Float) = 0
		_PatternOffset ("Pattern Offset", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}
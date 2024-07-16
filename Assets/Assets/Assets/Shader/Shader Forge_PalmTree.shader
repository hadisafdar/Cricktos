Shader "Shader Forge/PalmTree" {
	Properties {
		_WaveFrequency ("WaveFrequency", Float) = 1
		_WaveMagnitude ("WaveMagnitude", Float) = 1
		_FlutterMagnitude ("FlutterMagnitude", Float) = 1
		_FlutterFrequency ("FlutterFrequency", Float) = 1
		_Tex ("Tex", 2D) = "white" {}
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "ShaderForgeMaterialInspector"
}
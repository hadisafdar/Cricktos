Shader "Custom/Chrome_Simple" {
	Properties {
		_Reflections ("Reflections", Cube) = "_Skybox" {}
		_ReflectionMip ("ReflectionMip", Float) = 6
		_Colour ("Colour", Vector) = (1,0.602493,0,1)
		_Power ("Power", Float) = 1
		_Strength ("Strength", Float) = 1
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
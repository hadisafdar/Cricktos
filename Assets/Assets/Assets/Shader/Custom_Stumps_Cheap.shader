Shader "Custom/Stumps_Cheap" {
	Properties {
		_Color ("Color", Vector) = (0.5019608,0.5019608,0.5019608,1)
		_Spec ("Spec", Vector) = (0.5,0.5,0.5,1)
		_Brightness ("Brightness", Float) = 2
		[MaterialToggle] _Zing ("Zing", Float) = 1
		_Glow ("Glow", Vector) = (1,0,0,1)
		_Frequency ("Frequency", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "ShaderForgeMaterialInspector"
}
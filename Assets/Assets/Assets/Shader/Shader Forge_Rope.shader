Shader "Shader Forge/Rope" {
	Properties {
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_Color ("Color", Vector) = (0.5019608,0.5019608,0.5019608,1)
		_MainTex ("Base Color", 2D) = "white" {}
		_Specular ("Specular", Vector) = (0.5,0.5,0.5,1)
		_Dots ("Dots", Float) = 2
		_Tint ("Tint", Vector) = (0.5,0.5,0.5,1)
		_Brightness ("Brightness", Float) = 2
		_Power ("Power", Float) = 1
		_Speed ("Speed", Float) = 2
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "ShaderForgeMaterialInspector"
}
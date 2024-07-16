Shader "Custom/Billboard" {
	Properties {
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_ScaleX ("Scale X", Float) = 1
		_ScaleY ("Scale Y", Float) = 1
		_ScaleZ ("Scale Z", Float) = 1
		_Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
		_Cells ("X= Columns, Y=Rows, Z=Speed", Vector) = (8,8,60,0)
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
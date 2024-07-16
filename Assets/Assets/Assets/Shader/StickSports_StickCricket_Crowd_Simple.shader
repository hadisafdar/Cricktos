Shader "StickSports/StickCricket/Crowd_Simple" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		[MaterialToggle] _Celebrate ("Celebrate", Float) = 0
		_Amplitude ("Amplitude", Float) = 1
		_Frequency ("Frequency", Float) = 1
		_FlipbookSpeed ("FlipbookSpeed", Float) = 1
		_TeamColour ("TeamColour", Vector) = (0.5,0.5,0.5,1)
		_Skin ("Skin", Vector) = (0.503,0.3582684,0.253009,1)
		_LightmapStrength ("LightmapStrength", Float) = 1
		_Ambient ("Ambient", Float) = 0
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
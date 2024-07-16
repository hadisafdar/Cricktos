Shader "Custom/LEDs" {
	Properties {
		_Color ("Color", Vector) = (0.5019608,0.5019608,0.5019608,1)
		_Spec ("Spec", Vector) = (0.5,0.5,0.5,1)
		_Brightness ("Brightness", Float) = 1
		_Fog ("Fog", Vector) = (0.5,0.5,0.5,1)
		_LEDTex ("LEDTex", 2D) = "white" {}
		_Tile ("Tile", Float) = 0
		[MaterialToggle] _HideMainScreen ("HideMainScreen", Float) = 0
		[MaterialToggle] _HideSightScreen ("HideSightScreen", Float) = 0
		_TextAnimPower ("TextAnimPower", Float) = 4
		_SequenceTime ("SequenceTime", Float) = 2
		_TimeTest ("TimeTest", Range(0, 1)) = 1
		_TextTime ("TextTime", Float) = 1
		_Colour ("Colour", Vector) = (0.9528302,0.4163757,0.06741722,1)
		_TextColour ("TextColour", Vector) = (0.9056604,0.6877893,0.6877893,1)
		_GradientScaleMinMax ("GradientScaleMinMax", Vector) = (0,0,0,0)
		[MaterialToggle] _Flames ("Flames", Float) = 1
		_LogoColour ("LogoColour", Vector) = (0.4753938,0,1,1)
		_LogoBrightness ("LogoBrightness", Float) = 2
		_LogoTex ("LogoTex", 2D) = "white" {}
		_Logo1Offset ("Logo1Offset", Vector) = (1,1,0,0)
		_Logo2Offsset ("Logo2Offsset", Vector) = (1,1,0,0)
		_WideScale ("WideScale", Float) = 0
		[MaterialToggle] _WideDisplay ("WideDisplay", Float) = 0
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
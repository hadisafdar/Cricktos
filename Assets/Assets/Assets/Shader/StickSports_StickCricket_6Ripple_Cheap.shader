Shader "StickSports/StickCricket/6Ripple_Cheap" {
	Properties {
		_RingPos ("RingPos", Vector) = (0,0,0,0)
		_Rings ("Rings", Float) = 2
		_RipplePower ("RipplePower", Float) = 2
		_RingsPower ("RingsPower", Float) = 5
		[HDR] _RippleColour ("RippleColour", Vector) = (0.06,0.9,0.1,1)
		_RingSize ("RingSize", Float) = 1
		_TimeInput ("TimeInput", Float) = 0
		_Brightness ("Brightness", Float) = 1.5
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
}
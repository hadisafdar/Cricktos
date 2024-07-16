Shader "StickSports/StickCricket/6Ripple_Hex" {
	Properties {
		_RingPos ("RingPos", Vector) = (0,0,0,0)
		_Rings ("Rings", Float) = 2
		_RipplePower ("RipplePower", Float) = 2
		_RingsPower ("RingsPower", Float) = 5
		[HDR] _RippleColour ("RippleColour", Vector) = (0.06,0.9,0.1,1)
		_RingSize ("RingSize", Float) = 1
		_TimeInput ("TimeInput", Range(0, 1)) = 0
		_Brightness ("Brightness", Float) = 1.5
		_MainTex ("Hex Tex", 2D) = "grey" {}
		[HDR] _HexColour ("HexColour", Vector) = (0.06,0.9,0.1,1)
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
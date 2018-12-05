Shader "Custom/Diffuse Double Sided No Cull" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }

		////////////////////////////////////////////////////
		// First pass, render as usual
		////////////////////////////////////////////////////

		Cull Back

		CGPROGRAM
#pragma surface surf Lambert

		sampler2D _MainTex;
	fixed4 _Color;

	struct Input {
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		//c = fixed4(0, 1, 0, 1); // For simple debugging
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}

	ENDCG

		////////////////////////////////////////////////////
		// Second pass, flip normals and render other side
		////////////////////////////////////////////////////

		Cull Front

		CGPROGRAM
#pragma surface surf Lambert vertex:vert

		sampler2D _MainTex;
	fixed4 _Color;

	struct Input {
		float2 uv_MainTex;
	};

	void vert(inout appdata_full v, out Input o) {
		UNITY_INITIALIZE_OUTPUT(Input, o);
		v.normal = -v.normal;
	}

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		//c = fixed4(1, 0, 0, 1); // For simple debugging
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}

	ENDCG
	}

		Fallback "VertexLit"
}
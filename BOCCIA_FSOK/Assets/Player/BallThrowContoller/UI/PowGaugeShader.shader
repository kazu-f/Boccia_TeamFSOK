Shader "Custom/PowGaugeShader"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white"{}
		_GaugeColTex("GaugeColTex", 2D) = "white"{}
		_ThrowPow("ThrowPower",Range(0,1)) = 0.0
	}
		SubShader
	{
			Tags {"Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent"}
			Blend SrcAlpha OneMinusSrcAlpha Cull Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_throwPow
			#pragma fragment frag_throwPow
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _GaugeColTex;
			float _ThrowPow;

			struct input_vert
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert_throwPow(input_vert v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}

			float4 frag_throwPow(v2f i) : SV_Target
			{
				float4 finalCol = tex2D(_MainTex, i.uv);
				if (i.uv.y > 1.0f - _ThrowPow) {
					finalCol.xyz = tex2D(_GaugeColTex, i.uv).xyz;
				}

				return finalCol;
			}
			ENDCG
		}
	}
}

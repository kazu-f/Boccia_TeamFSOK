Shader "Hidden/EmphasisShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            Tags {"RenderType" = "Opaque"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float2 uv : TEXCOORD0;
            };

            sampler2D _CameraDepthNormalsTexture;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            
            fixed3 CameraForward;
            fixed4 frag (v2f i) : SV_Target
            {
                // _CameraDepthNormalsTextureからサンプリング
                float4 cdn = tex2D(_CameraDepthNormalsTexture, i.uv);
                // サンプリングした値をDecodeViewNormalStereo()で変換
                float3 normal = DecodeViewNormalStereo(cdn) * float3(1.0, 1.0, -1.0);
                normal = normalize(normal);
                fixed4 col = tex2D(_MainTex, i.uv);
                //カメラの前方向
                //float3 forward = UNITY_MATRIX_V[2].xyz;
                float3 forward = CameraForward;
                //forward = normalize(forward);
                if (dot(normal, forward) < 0.1)
                {
                    fixed red = 0.0;
                    fixed green = 0.0;
                    fixed blue = 0.0;
                    fixed alpha = 1.0;
                    return fixed4(red, green, blue, alpha);
                }
                return col;
            }
            ENDCG
        }
    }
}

Shader "Unlit/OutlineGlow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth ("Outline Width", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #define DIV_SQRT_2 0.70710678118

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _OutlineColor;
            float _OutlineWidth;
            float4 _MainTex_TexelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 directions[8] = {float2(1, 0), float2(0, 1), float2(-1, 0), float2(0, -1), 
                    float2(DIV_SQRT_2, DIV_SQRT_2), float2(-DIV_SQRT_2, DIV_SQRT_2), float2(-DIV_SQRT_2, -DIV_SQRT_2), float2(DIV_SQRT_2, -DIV_SQRT_2)};

                float2 sampleDistance = _MainTex_TexelSize.xy * _OutlineWidth;

                float maxAlpha = 0;
                for (uint index = 0; index < 8; index++) 
                {
                    float2 sampleUV = i.uv + directions[index] * sampleDistance;
                    maxAlpha = max(maxAlpha, tex2D(_MainTex, sampleUV).a);
                }

                col.rgb = lerp(_OutlineColor, col.rgb, col.a);
                col.a = max(col.a, maxAlpha);

                return col;
            }
            ENDCG
        }
    }
}

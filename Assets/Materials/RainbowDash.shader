
Shader "Custom/RainbowMaterial"
{
    Properties
    {
        _Speed ("Rainbow Speed", Range(0, 5)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        Pass
        {
            Cull Back
            ZWrite On

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _Speed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float phase = i.uv.x + _Time.y * _Speed;

                float3 rainbow = 0.5 + 0.5 * cos(6 * (phase + float3(0.0, 0.3333333, 0.6666667)));

                return fixed4(rainbow, 1.0);
            }

            ENDCG
        }
    }
}

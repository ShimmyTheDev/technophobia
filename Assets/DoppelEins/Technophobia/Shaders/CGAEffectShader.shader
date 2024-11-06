Shader "Custom/CGAColorPalette"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HighIntensity ("High Intensity", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Name "CGA Color Pass"
            ZTest Always Cull Off ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _HighIntensity;

            // CGA palette for Mode 4 in RGB format (0-1 range).
            static const float4 colors[16] = {
                float4(0.0, 0.0, 0.0, 1.0),       // Black
                float4(0.0, 0.0, 0.667, 1.0),     // Blue
                float4(0.0, 0.667, 0.0, 1.0),     // Green
                float4(0.0, 0.667, 0.667, 1.0),   // Cyan
                float4(0.667, 0.0, 0.0, 1.0),     // Red
                float4(0.667, 0.0, 0.667, 1.0),   // Magenta
                float4(0.667, 0.333, 0.0, 1.0),   // Brown
                float4(0.667, 0.667, 0.667, 1.0), // Light Gray
                float4(0.333, 0.333, 0.333, 1.0), // Dark Gray
                float4(0.333, 0.333, 1.0, 1.0),   // Light Blue
                float4(0.333, 1.0, 0.333, 1.0),   // Light Green
                float4(0.333, 1.0, 1.0, 1.0),     // Light Cyan
                float4(1.0, 0.333, 0.333, 1.0),   // Light Red
                float4(1.0, 0.333, 1.0, 1.0),     // Light Magenta
                float4(1.0, 1.0, 0.333, 1.0),     // Yellow
                float4(1.0, 1.0, 1.0, 1.0)        // White
            };

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 nearestColor(float4 color)
            {
                // Find the closest color in the palette
                float4 bestColor = colors[0];
                float minDist = distance(color, bestColor);

                for (int i = 1; i < 16; i++)
                {
                    float dist = distance(color, colors[i]);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        bestColor = colors[i];
                    }
                }
                return bestColor;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 color = tex2D(_MainTex, i.uv);
                return nearestColor(color);
            }

            ENDHLSL
        }
    }
    FallBack "Diffuse"
}

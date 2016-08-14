Shader "ShaderX2"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _SwapTex("Color Data", 2D) = "transparent" {}
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
    }


    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ PIXELSNAP_ON
#include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
            };

            fixed4 _Color;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

                return OUT;
            }

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float _AlphaSplitEnabled;

            sampler2D _SwapTex;

            fixed4 SampleSpriteColorTexture(float2 uv)
            {
                fixed4 colorTexture = tex2D(_MainTex, uv);
                if (_AlphaSplitEnabled)
                {
                    colorTexture.a = tex2D(_AlphaTex, uv).r;
                }
                return colorTexture;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 colorTexture = SampleSpriteColorTexture(IN.texcoord);
                fixed4 swapCol = tex2D(_SwapTex, float2(colorTexture.x, 0));
                fixed4 final = lerp(colorTexture, swapCol, swapCol.a) * IN.color;
                final.a = colorTexture.a;
                final.rgb *= colorTexture.a;
                return final;
            }
            ENDCG
        }
    }
}

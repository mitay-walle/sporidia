Shader "UI/Chromatic Aberration Glitch"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _UVNoiseMask ("UV Noise Mask", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _ChrommaticAberration ("Chrommatic Aberration", Float) = 1
        _UVGlitch ("UV Glitch", Vector) = (1,1,0,0)
        _GlitchMoveSpeed ("Glitch Move Speed", Float) = 10
        _GlitchFlashSpeed ("Glitch Flash Speed", Float) = 0.9
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 texcoord2 : TEXCOORD2;
                float4 texcoord3 : TEXCOORD3;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                half2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float4 texcoord2 : TEXCOORD2;
                float4 texcoord3 : TEXCOORD3;
            };

            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            sampler2D _UVNoiseMask;
            fixed4 _UVNoiseMask_ST;
            fixed _GlitchMoveSpeed;
            fixed _GlitchFlashSpeed;
            fixed _ChrommaticAberration;
            fixed4 _UVGlitch;

            bool _UseClipRect;
            float4 _ClipRect;

            bool _UseAlphaClip;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.worldPosition = IN.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = IN.texcoord;
                OUT.texcoord2 = IN.texcoord2;
                OUT.texcoord3.xy = TRANSFORM_TEX(IN.texcoord, _UVNoiseMask);

                #ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
                #endif

                OUT.color = IN.color * _Color;
                return OUT;
            }

            sampler2D _MainTex;

            float random(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453123);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Glitch UV
                half value = 0;
                if (round(random(saturate(frac(_Time.y + random(_Time.y)) - _GlitchFlashSpeed))) > 0)
                {
                    value = 1;
                    i.texcoord3.x += _Time.y * _GlitchMoveSpeed;
                    float4 offset2 = tex2D(_UVNoiseMask, i.texcoord3*_UVGlitch );
                    i.texcoord += offset2.rg * offset2.a * _UVGlitch.y/100;
                }


                // Chromatic Aberration 
                float offset = (_ChrommaticAberration + i.texcoord2.x)/100 * value;
                float2 uvR = float2(i.texcoord.x - offset, i.texcoord.y);
                float2 uvG = float2(i.texcoord.x + offset, i.texcoord.y);
                float2 uvB = float2(i.texcoord.x, i.texcoord.y - offset);
                fixed4 colR = tex2D(_MainTex, uvR);
                fixed4 colG = tex2D(_MainTex, uvG);
                fixed4 colB = tex2D(_MainTex, uvB);
                float4 color = fixed4(colR.r * colR.a, colG.g * colG.a, colB.b * colB.a,
                                      (colR.a + colG.a + colB.a) / 3);


                color *= i.color;
                if (_UseClipRect)
                    color *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);

                if (_UseAlphaClip)
                    clip(color.a - 0.001);

                return color;
            }
            ENDCG
        }
    }
    FallBack "UI/Default"
}
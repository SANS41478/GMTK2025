Shader "Hidden/URP/GlobalMask"
{
    Properties
    {
        _MaskTex1 ("Mask Texture 1", 2D) = "white" {}
        _MaskTex2 ("Mask Texture 2", 2D) = "white" {}
        _MaskTex3 ("Mask Texture 3", 2D) = "white" {}
        _MaskTex4 ("Mask Texture 4", 2D) = "white" {}
        _SwitchSpeed ("Switch Speed", Range(0, 10)) = 1.0
        _BlendMode ("Blend Mode (0=Normal,1=Multiply)", Float) = 0
        _Opacity   ("Mask Opacity", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" }

        // Pass 0 for Normal Blending
        Pass
        {
            Name "Normal"
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
            };

            TEXTURE2D(_MaskTex1); SAMPLER(sampler_MaskTex1);
            TEXTURE2D(_MaskTex2); SAMPLER(sampler_MaskTex2);
            TEXTURE2D(_MaskTex3); SAMPLER(sampler_MaskTex3);
            TEXTURE2D(_MaskTex4); SAMPLER(sampler_MaskTex4);
            float _Opacity;
            float _SwitchSpeed;

            Varyings Vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                // 根据时间随机选择一个遮罩
                float timeStep = floor(_Time.y * _SwitchSpeed);
                float rand = frac(sin(timeStep) * 43758.5453);
                int texIndex = floor(rand * 4);

                half4 maskCol;
                if (texIndex == 0)      maskCol = SAMPLE_TEXTURE2D(_MaskTex1, sampler_MaskTex1, IN.uv);
                else if (texIndex == 1) maskCol = SAMPLE_TEXTURE2D(_MaskTex2, sampler_MaskTex2, IN.uv);
                else if (texIndex == 2) maskCol = SAMPLE_TEXTURE2D(_MaskTex3, sampler_MaskTex3, IN.uv);
                else                    maskCol = SAMPLE_TEXTURE2D(_MaskTex4, sampler_MaskTex4, IN.uv);

                float maskAlpha = maskCol.a * _Opacity;
                return half4(maskCol.rgb, maskAlpha);
            }
            ENDHLSL
        }

        // Pass 1 for Multiply Blending
        Pass
        {
            Name "Multiply"
            ZWrite Off
            Cull Off
            Blend DstColor Zero

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
            };

            TEXTURE2D(_MaskTex1); SAMPLER(sampler_MaskTex1);
            TEXTURE2D(_MaskTex2); SAMPLER(sampler_MaskTex2);
            TEXTURE2D(_MaskTex3); SAMPLER(sampler_MaskTex3);
            TEXTURE2D(_MaskTex4); SAMPLER(sampler_MaskTex4);
            float _Opacity;
            float _SwitchSpeed;

            Varyings Vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                // 根据时间随机选择一个遮罩
                float timeStep = floor(_Time.y * _SwitchSpeed);
                float rand = frac(sin(timeStep) * 43758.5453);
                int texIndex = floor(rand * 4);

                half4 maskCol;
                if (texIndex == 0)      maskCol = SAMPLE_TEXTURE2D(_MaskTex1, sampler_MaskTex1, IN.uv);
                else if (texIndex == 1) maskCol = SAMPLE_TEXTURE2D(_MaskTex2, sampler_MaskTex2, IN.uv);
                else if (texIndex == 2) maskCol = SAMPLE_TEXTURE2D(_MaskTex3, sampler_MaskTex3, IN.uv);
                else                    maskCol = SAMPLE_TEXTURE2D(_MaskTex4, sampler_MaskTex4, IN.uv);

                float maskAlpha = maskCol.a * _Opacity;
                half3 finalColor = lerp(half3(1,1,1), maskCol.rgb, maskAlpha);
                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack Off
}

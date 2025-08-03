Shader "Custom/URP2D/SpriteDuotone"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _DarkColor("Dark Color", Color) = (0, 0, 0, 1)
        _LightColor("Light Color", Color) = (1, 1, 1, 1)
        _GlitchStrength("Glitch Strength", Range(0, 1)) = 0.1
        _GlitchSpeed("Glitch Speed", Range(0, 10)) = 1.0
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "CanUseSpriteAtlas"="True"
        }
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "DUOTONE"
            Tags { "LightMode"="Universal2D" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float4 color       : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _DarkColor;
            float4 _LightColor;
            float _GlitchStrength;
            float _GlitchSpeed;

            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_INSTANCING_BUFFER_END(Props)

            // Simple pseudo-random function
            float rand(float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv          = IN.uv;
                OUT.color       = IN.color;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                // Glitch Effect
                float glitchTime = _Time.y * _GlitchSpeed;
                float glitch = rand(float2(glitchTime, glitchTime));
                if (glitch > 0.95) // Trigger glitch occasionally
                {
                    float offset = rand(float2(glitchTime * 2.0, glitchTime * 2.0)) * _GlitchStrength;
                    uv.x += offset;
                }

                float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv) * IN.color;
                
                // Chromatic Aberration part of the glitch
                if (glitch > 0.9)
                {
                    float r_offset = rand(float2(glitchTime * 1.3, glitchTime * 1.3)) * 0.02 * _GlitchStrength;
                    float b_offset = rand(float2(glitchTime * 1.7, glitchTime * 1.7)) * 0.02 * _GlitchStrength;
                    tex.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(r_offset, 0)).r;
                    tex.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv - float2(b_offset, 0)).b;
                }

                float lum  = dot(tex.rgb, float3(0.299, 0.587, 0.114));
                float3 col = lerp(_DarkColor.rgb, _LightColor.rgb, lum);
                return float4(col, tex.a);
            }
            ENDHLSL
        }
    }
    FallBack "Sprites/Default"
}

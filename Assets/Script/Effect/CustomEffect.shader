Shader "Custom/CustomEffect"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

            #pragma vertex vert
            #pragma fragment frag

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float _Intensity;
            float4 _OverlayColor;

            float _Dx;
            float _Dy;
            float _Intercept;
            float _Slash;
            float _Decompose;

            struct Attributes
            {
                float4 positionOS       : POSITION;
                float2 uv               : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv        : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };


            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.vertex = vertexInput.positionCS;
                output.uv = input.uv;

                return output;
            }

            float4 frag(Varyings i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                float2 pos = i.uv;
                float2 delta = float2(_Dx * _Slash, _Dy * _Slash);
                if (i.uv.y <= _Dy / _Dx * i.uv.x + _Intercept) {
                    pos += delta;
                    pos.y += _Decompose;
                }
                else {
                    pos -= delta;
                    pos.y -= _Decompose;
                }

                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, pos);
            }

            ENDHLSL
        }
    }
        FallBack "Diffuse"
}
Shader "Custom/EdgeHighlightShader2"
{
    Properties
    {
        _EdgeColor ("Edge Color", Color) = (1,0,0,1) // Red color for edges
        _EdgeWidth ("Edge Width", Range(0.01, 0.5)) = 0.05 // Edge width control
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _EdgeColor;
                float _EdgeWidth;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.normalWS = normalWS;
                OUT.viewDirWS = normalize(_WorldSpaceCameraPos - TransformObjectToWorld(IN.positionOS.xyz));
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float rim = 1.0 - saturate(dot(normalize(IN.normalWS), normalize(IN.viewDirWS)));
                float edge = smoothstep(_EdgeWidth * 0.5, _EdgeWidth, 1 - rim);
                float alpha = 1.0 - step(0.8, edge); // Adjusted to correct edge detection
                return float4(_EdgeColor.rgb, alpha * _EdgeColor.a); // Apply color and transparency based on alpha
            }
            ENDHLSL
        }
    }
    FallBack "Transparent"
}

Shader "Custom/EdgeHighlightShader"
{
    Properties
    {
        _EdgeColor ("Edge Color", Color) = (1,0,0,1) // Red color for edges
        _EdgeWidth ("Edge Width", Range(0.01, 1.0)) = 0.1 // Sensitivity of edge width
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
                float edge = smoothstep(0.0, _EdgeWidth, rim);
                float alpha = step(0.5, edge); // Create a hard threshold for visibility
                float3 color = lerp(float3(0,0,0), _EdgeColor.rgb, edge);
                return float4(color, alpha); // Use alpha for transparency control
            }
            ENDHLSL
        }
    }
    FallBack "Transparent"
}

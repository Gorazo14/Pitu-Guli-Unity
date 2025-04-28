Shader "Custom/TwoSidedPBR"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _MetallicGlossMap ("Metallic (R) & Smoothness (A)", 2D) = "white" {}
        _ParallaxMap ("Height Map", 2D) = "black" {}
        
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Parallax ("Height Scale", Range(0.005, 0.08)) = 0.02
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 300

        Cull Off   // ✅ Disable backface culling

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _MetallicGlossMap;
        sampler2D _ParallaxMap;

        half _Metallic;
        half _Glossiness;
        float _Parallax;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float2 uv_MetallicGlossMap;
            float2 uv_ParallaxMap;
            float3 viewDir;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Parallax Mapping (fake displacement)
            half height = tex2D(_ParallaxMap, IN.uv_ParallaxMap).r;
            float2 parallaxOffset = ParallaxOffset(height, _Parallax, IN.viewDir);
            IN.uv_MainTex += parallaxOffset;
            IN.uv_BumpMap += parallaxOffset;
            IN.uv_MetallicGlossMap += parallaxOffset;

            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;

            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

            fixed4 metallicGloss = tex2D(_MetallicGlossMap, IN.uv_MetallicGlossMap);
            o.Metallic = metallicGloss.r * _Metallic;
            o.Smoothness = metallicGloss.a * _Glossiness;

            // Optional: Fix weird lighting from backside
            if (dot(IN.viewDir, o.Normal) < 0)
                o.Normal = -o.Normal;
        }
        ENDCG
    }

    FallBack "Standard"
}

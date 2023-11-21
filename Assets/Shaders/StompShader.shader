Shader "Custom/CircularGradientTransparent"
{
    Properties
    {
        _MainTex ("Gradient Texture", 2D) = "white" { }
        _Color ("Color Multiplier", Color) = (1,1,1,1)
        _Radius ("Radius", Range(0.0, 1.0)) = 0.5
    }

    CGINCLUDE
    #include "UnityCG.cginc"
    ENDCG

    SubShader
    {
        Tags { "Queue" = "Overlay" }

        Blend SrcAlpha OneMinusSrcAlpha // Enable alpha blending

        CGPROGRAM
        #pragma surface surf Lambert alpha

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed4 _Color;
        float _Radius;

        void surf (Input IN, inout SurfaceOutput o)
        {
            float dist = distance(IN.uv_MainTex, 0.5) / (_Radius-0.475);
            fixed4 c = tex2D(_MainTex, float2(dist,dist)) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a * pow(1-clamp(_Radius,0,1),0.4f);
        }
        ENDCG
    }

    FallBack "Diffuse"
}

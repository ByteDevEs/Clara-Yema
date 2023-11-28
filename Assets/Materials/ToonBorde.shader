Shader "Unlit/ToonBorde"
{
    Properties
    {
        _InkColor("InkColor", Color) = (0,0,0,0)
        _InkSize("InkSize", float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            //Solo las caras frontales
            Cull Front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float _InkColor;
            float _InkSize;

            v2f vert (appdata v)
            {
                v2f o;
                //Esto hace que el objeto sea más grande para que sobresalga del original
                o.vertex = UnityObjectToClipPos(v.vertex + _InkSize * v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _InkColor;
            }
            ENDCG
        }
    }
}
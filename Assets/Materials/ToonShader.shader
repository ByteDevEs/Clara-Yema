Shader "Unlit/ToonShader"
{
    Properties
    {
        _Albedo("Albedo", Color) = (1,1,1,1)
        _Shades("Shades", Range(1,20)) = 3
        _InkColor("InkColor", Color) = (0,0,0,1) // Cambiado a Color y valor predeterminado no transparente
        _InkSize("InkSize", float) = 1.0
       
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
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

            fixed4 _InkColor; // Cambiado a fixed4 para representar un color

            float _InkSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex + _InkSize * v.normal);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _InkColor; // Devuelve el color de la tinta
            }
            ENDCG
        }
    
        Pass
        {
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
                float3 worldNormal : TEXCOORD0;
            };

            float4 _Albedo;
            float _Shades;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //Calcula el coseno del ángulo entre el vector normal y la dirección de la luz
                float cosineAngle = dot(normalize(i.worldNormal), normalize(_WorldSpaceLightPos0.xyz));

                cosineAngle = max(cosineAngle, 0.0);
                cosineAngle = floor(cosineAngle * _Shades) / _Shades;
                return _Albedo * cosineAngle;
            }
            ENDCG
        }
    }
    Fallback "VertexLit"
}

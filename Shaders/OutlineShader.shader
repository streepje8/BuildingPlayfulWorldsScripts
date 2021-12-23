/*
*
* Outline Shader
* Wessel Roelofse
* A shader for applying outlines to meshes
* 
*/
Shader "Unlit/OutlineShader"
{
    Properties
    {
        [HDR]_BaseColor("Color", Color) = (1,1,1,1)
        [HDR]_OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline size", Float) = 1
    }
    SubShader
    {
        Tags {
            "RenderType" = "Opaque"
            "LightMode" = "ForwardBase"
            "PassFlags" = "OnlyDirectional"
            "Queue" = "Geometry"
        }
        LOD 100

        ZWrite On

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };
            float4 _BaseColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _BaseColor;
            }
            ENDCG
        }

        //Outline Pass
        Pass
        {
            Cull FRONT

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            half _OutlineWidth;
            float4 _OutlineColor;

            v2f vert(appdata v)
            {
                float3 worldScale = float3(
                    length(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x)), // scale x axis
                    length(float3(unity_ObjectToWorld[0].y, unity_ObjectToWorld[1].y, unity_ObjectToWorld[2].y)), // scale y axis
                    length(float3(unity_ObjectToWorld[0].z, unity_ObjectToWorld[1].z, unity_ObjectToWorld[2].z))  // scale z axis
                    );
                //Invert de object scale
                float3 scale = 1 / worldScale;
                v.vertex.xyz += v.vertex.xyz * _OutlineWidth * scale;
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag() : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

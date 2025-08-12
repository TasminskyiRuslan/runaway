// Shader "Custom/ObjectHiding"
// {
//     Properties
//     {
//         _Color ("Color", Color) = (1,1,1,1)
//         _MainTex ("Albedo (RGB)", 2D) = "white" {}
//         _Glossiness ("Smoothness", Range(0,1)) = 0.5
//         _Metallic ("Metallic", Range(0,1)) = 0.0
//         _HidingDistance("Hide Distance", Range(1,10)) = 5
//     }
//     SubShader
//     {
//         Tags { "RenderType"="Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True" }
//         Blend SrcAlpha OneMinusSrcAlpha
//         ZWrite On
//         LOD 200

//         CGPROGRAM
//         #pragma surface surf Standard fullforwardshadows alpha:fade
//         #pragma target 3.0

//         sampler2D _MainTex;

//         struct Input
//         {
//             float2 uv_MainTex;
//             float3 worldPos;
//         };

//         half _Glossiness;
//         half _Metallic;
//         fixed4 _Color;

//         float3 _PlayerWorldPos;
//         float3 _CameraWorldPos;
//         float _HidingDistance;

//         UNITY_INSTANCING_BUFFER_START(Props)
//         UNITY_INSTANCING_BUFFER_END(Props)

//         void surf (Input IN, inout SurfaceOutputStandard o)
//         {
//             fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
//             float3 toPlayer = _PlayerWorldPos - IN.worldPos;
//             float xz = length(toPlayer.xz);

//             float addAlpha = smoothstep(-1, 0, toPlayer.y);
//             float alpha = clamp(0, 1, smoothstep(0, _HidingDistance, xz / _HidingDistance) + addAlpha);

//             o.Albedo = c.rgb;
//             o.Smoothness = _Glossiness;  
//             o.Alpha = alpha;
//         }
//         ENDCG
//     }
//     FallBack "Diffuse"
// }
Shader "Custom/ObjectHiding"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HolePosition ("Hole Position", Vector) = (0,0,0,0)
        _HoleRadius ("Hole Radius", Float) = 1
        _Color ("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _HolePosition;
            float _HoleRadius;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float dist = distance(i.worldPos.xz, _HolePosition.xz);
                float alpha = 1;

                if (dist < _HoleRadius)
                {
                    alpha = saturate((dist - (_HoleRadius * 0.5)) / (_HoleRadius * 0.5));
                }

                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.a *= alpha;
                return col;
            }
            ENDCG
        }
    }
}

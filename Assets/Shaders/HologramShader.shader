Shader "Custom/HologramCone"
{
    Properties
    {
        _Color ("Main Color", Color) = (0, 1, 1, 1)
        _RimColor ("Rim Color", Color) = (0.5, 1, 1, 1)
        _RimPower ("Rim Power", Range(0.5, 8.0)) = 3.0
        _ScrollSpeed ("Scroll Speed", Range(0, 10)) = 1.0
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Lighting Off
        Cull Back

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            float _ScrollSpeed;
            float4 _Color;
            float4 _RimColor;
            float _RimPower;

            sampler2D _MainTex;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                o.worldNormal = normalize(mul(v.normal, (float3x3)unity_ObjectToWorld));

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Base color from texture
                float2 uvScroll = i.uv;
                uvScroll.y += _Time.y * _ScrollSpeed; // Scroll effect
                float4 texColor = tex2D(_MainTex, uvScroll);
                
                // Rim lighting effect for hologram edges
                float rimFactor = 1.0 - saturate(dot(i.viewDir, i.worldNormal));
                rimFactor = pow(rimFactor, _RimPower);
                
                // Combine base color and rim color
                float4 finalColor = _Color * texColor;
                finalColor.rgb += rimFactor * _RimColor.rgb;

                // Final output with alpha for transparency
                finalColor.a = _Color.a * texColor.a;

                return finalColor;
            }
            ENDCG
        }
    }
    
    // Fallback for low-end systems
    Fallback "Transparent/VertexLit"
}

Shader "LuB/NewToon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _LitTrashHold ("Lit Trashhold", Range(-1,1)) = 0
        _LitSoftness ("Lit Softness", Range(0,1)) = 0
        _ShadingColor ("Shading Color", Color) = (1,1,1,1)
        _SpecularColor("Specular Color", Color) = (1,1,1,1)
        _SpecularSize ("Specular Size", Range(0,1)) = 0
        _SpecularSoftness ("Specular Softness", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _Color;

            float _LitTrashHold;
            float _LitSoftness;
            fixed4 _ShadingColor;
            
            fixed4 _SpecularColor;
            float _SpecularSize;
            float _SpecularSoftness;

            v2f vert (appdata v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                float fong = dot(_WorldSpaceLightPos0.xyz, normalize(i.worldNormal));

                float spec = smoothstep(1-_SpecularSize, 1-(1-_SpecularSoftness)*_SpecularSize, fong);

                fong = smoothstep(_LitTrashHold - _LitSoftness*2, _LitTrashHold + _LitSoftness, fong);

                fong = saturate(fong);

                col = lerp(_ShadingColor,col,fong);
                
                return col + spec * _SpecularColor;
            }
            ENDCG
        }
    }
}

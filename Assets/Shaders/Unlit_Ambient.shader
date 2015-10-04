// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "Custom/Mobile/AmbientShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			Lighting Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF

			#include "UnityCG.cginc"

			struct appdata_lightmap {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			// sampler2D unity_Lightmap;
			// float4 unity_LightmapST;

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata_lightmap v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv0 = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv1 = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv0);
				#ifdef LIGHTMAP_ON
				col.xyz *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1));
				#else
				col *= UNITY_LIGHTMODEL_AMBIENT;
				#endif
				return col;
			}
			ENDCG
		}
	}
}

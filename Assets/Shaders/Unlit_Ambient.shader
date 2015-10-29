Shader "Custom/Mobile/Environment (Ambient)"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry-1" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF
			#pragma multi_compile IS_SIMPLE IS_ADVANCED
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"
			#ifdef IS_ADVANCED
			#include "Utility.cginc"
			#endif

			struct appdata_lightmap {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				
				#ifdef IS_ADVANCED
				float4 normal : NORMAL;
				#endif
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				
				#ifdef IS_ADVANCED
				float3 vertexWorld : TEXCOORD2;
				float3 normal : TEXCOORD3;
				#endif
			};

			float4 _MainTex_ST;
			
			v2f vert (appdata_lightmap v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv0 = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv1 = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				
				#ifdef IS_ADVANCED
				o.vertexWorld = mul(_Object2World,v.vertex );
				o.normal = mul(_Object2World, v.normal);
				#endif

				return o;
			}
			
			sampler2D _MainTex;
			
			fixed4 frag (v2f i) : COLOR
			{	
				fixed3 light = fixed3(1,1,1);
				#ifdef LIGHTMAP_ON
				light *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1));
				#else
				light *= UNITY_LIGHTMODEL_AMBIENT;
				#endif
				
				#ifdef IS_ADVANCED
				light += FlashLightColor(i.vertexWorld, i.normal, _FlashLight_Position, _FlashLight_Forward, _FlashLight_Color, _FlashLight_Params);
				#endif
				
				fixed4 col = tex2D(_MainTex, i.uv0);
				
				return fixed4(light*col.rgb, 1.0);
			}
			ENDCG
		}
	}
}

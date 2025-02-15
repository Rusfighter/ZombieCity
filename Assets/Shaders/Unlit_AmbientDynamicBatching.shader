﻿Shader "Custom/Mobile/Dynamic (Ambient + Unlit + Batching)"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry-2" "Batching"="Dynamic" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile IS_SIMPLE IS_ADVANCED

			#include "UnityCG.cginc"
			
			#ifdef IS_ADVANCED
			#include "Utility.cginc"
			#endif

			struct appdata_lightmap {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				
				fixed3 selector : TANGENT;
				
				#ifdef IS_ADVANCED
				float4 normal : NORMAL;
				#endif
			};

			struct v2f
			{
				float2 uv0 : TEXCOORD0;
				float4 vertex : SV_POSITION;
				
				#ifdef IS_ADVANCED
				float3 vertexWorld : TEXCOORD2;
				float3 normal : TEXCOORD3;
				#endif
			};

			float4 _MainTex_ST;
			uniform float4x4 ModelMatrix[20];
			
			v2f vert (appdata_lightmap v)
			{
				v2f o;
				
				float4x4 modelMatrix = ModelMatrix[v.selector.x];
				
				o.vertex = mul(mul(UNITY_MATRIX_VP, modelMatrix), v.vertex);
				o.uv0 = TRANSFORM_TEX(v.texcoord, _MainTex);

				#ifdef IS_ADVANCED
				o.vertexWorld = mul(modelMatrix, v.vertex);
				o.normal = mul(modelMatrix, v.normal);
				#endif

				return o;
			}
			
			sampler2D _MainTex;
			
			fixed4 frag (v2f i) : COLOR
			{
			
				fixed3 light = fixed3(1,1,1);
				light *= UNITY_LIGHTMODEL_AMBIENT;
				
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

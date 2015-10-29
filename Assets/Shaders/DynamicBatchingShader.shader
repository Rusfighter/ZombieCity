Shader "Custom/Batching/Dynamic Batching"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		// We use Forward base light model for this example
		Tags{
			"Queue"="Geometry" "RenderType"="Opaque" "LightMode"="ForwardBase"
		}
		
		Pass
		{
			CGPROGRAM
			
			///////////////// PRAGMAS /////////////////
			
			#pragma vertex VertexProgram
			#pragma fragment FragmentProgram
			
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0
			
			#include "Lighting.cginc"

			struct VertexInput
			{
				float4 position : POSITION;
				half2 texcoord0 : TEXCOORD0;
				// Lighting
				half3 normal : NORMAL;
				// This will be our selector
				fixed3 selector : TANGENT;
			};
			
			struct FragmentInput
			{
				float4 position : SV_POSITION;
				half2 texcoord0 : TEXCOORD0;
				half3 normal : TEXCOORD1;
			};
			
			/////////////////// UNIFORMS ///////////////////
			
			uniform sampler2D _MainTex;
			uniform fixed4 _Color;
			uniform float _BatchingCount;
			
			uniform float4x4 ModelMatrix[20];
			
			////////////////// PROGRAMS ////////////////////
			
			FragmentInput VertexProgram(VertexInput input)
			{
				FragmentInput output = (FragmentInput)0;

				float4x4 modelMatrix = ModelMatrix[input.selector.x];

				output.position = mul(mul(UNITY_MATRIX_VP, modelMatrix), input.position);
				output.texcoord0 = input.texcoord0;
					// We use the normal in the world space, so do the transformation to the world space
				output.normal = mul(modelMatrix, input.normal);
				
				return output;
			}
			
			fixed4 FragmentProgram(FragmentInput input) : COLOR
			{
				fixed4 tex = tex2D(_MainTex, input.texcoord0) * _Color;
				
				// This lighting works only with one directional light
				fixed4 diffuse = saturate(dot(_WorldSpaceLightPos0.xyz, input.normal));
				
				return tex * diffuse * _LightColor0 + UNITY_LIGHTMODEL_AMBIENT;
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}

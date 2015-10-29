// The include utilities

uniform float3 _FlashLight_Position;
uniform float3 _FlashLight_Forward;
uniform fixed4 _FlashLight_Color;
uniform float4 _FlashLight_Params;

inline fixed3 FlashLightColor(float3 vpos, float3 vnormal, float3 flash_pos, float3 flash_forward, float4 flash_color, float4 flash_params){
	float dotP = saturate(dot(flash_forward, normalize(vpos-flash_pos)) - flash_params.w);
	fixed flashStrength = lerp(0,flash_color.a, dotP);
	//dont lit back of objects
	flashStrength *= lerp(0, 1, saturate(-dot(vnormal, flash_forward)));
	//distance	
	flashStrength *= lerp(1, 0, length(flash_pos-vpos)/ flash_params.x);
	
	return saturate(flash_color.rgb * flashStrength * flash_params.z*8.0);
}

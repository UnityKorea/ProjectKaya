void MainLight_half(float3 WorldPos, out half3 Direction, out half3 Color, out half DistanceAtten, out half ShadowAtten)
{
	#ifdef SHADERGRAPH_PREVIEW
		Direction = half3(0.5, 0.5, 0);
		Color = 1;
		DistanceAtten = 1;
		ShadowAtten = 1;
	#else
		half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
		Light mainLight = GetMainLight(shadowCoord);
		Direction = mainLight.direction;
		Color = mainLight.color;
		DistanceAtten = mainLight.distanceAttenuation;
	
		#if !defined(_MAIN_LIGHT_SHADOWS) || defined(_RECEIVE_SHADOWS_OFF)
			ShadowAtten = 1.0h;
		#endif
			ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
			half shadowStrength = GetMainLightShadowStrength();
			ShadowAtten = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowSamplingData, shadowStrength, false);
	#endif
}




void AdditionalLights_half(half3 WorldPos, half3 WorldNormal, half3 WorldView, out half3 Diffuse)
{
	half3 diffuseColor = 0;
	
#ifndef SHADERGRAPH_PREVIEW
	WorldNormal = normalize(WorldNormal);
	WorldView = SafeNormalize(WorldView);
	int pixelLightCount = GetAdditionalLightsCount();
	for (int i = 0; i < pixelLightCount; ++i)
	{
		Light light = GetAdditionalLight(i, WorldPos);
		half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
		diffuseColor += LightingLambert(attenuatedLightColor, light.direction, WorldNormal);
	}
#endif

	Diffuse = diffuseColor;
}
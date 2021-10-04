Shader "UTKTemplate/URPHairKajiyaKay"
{
	Properties
	{
	 [Header(Hair shader for URP)]
	 [Space(20)]
	 [HDR]_TintColor("Tint Color", color) = (1,1,1,1)
	 [NoScaleOffset]_BaseMap("Albedo(RGB), Alpha(A)", 2D) = "white" {}
	 
	 [Header(Normal mapping)]
	 [Space(5)]
	 [Toggle] _Normalmap("Use Normal map", float) = 0
     _NormalInten("Normal Intensity", Range(0,3)) = 1
	 [Normal][NoScaleOffset]_BumpMap("Normal texture", 2D) = "white" {}	 	 

	 [Header(Flowmap ShiftMap)]
	 [Space(10)]	 	 
	 [NoScaleOffset]_Flowmap("flow texture", 2D) = "white" {}
	 [NoScaleOffset]_Shiftmap("shift texture", 2D) = "white" {}
	 
	 [Header(Lighting Control slide)][Space(10)]
	 _Shift("Specular Shift", int) = 0
     _Aniso("Specular Range", int) = 1
	 _AnisoIntensity("Specular Intensity", Range(0, 30)) = 1
	 
	 [Header(GI Term)][Space(10)]
	 _EnviIntensity("Environment Lighting", Range(0,1)) = 1
	 _Frensnel("Fresnel Range", Range(0.001, 1)) = 0.05
	 _FrensnelTerm("Fresnel Intensity", Range(0, 5)) = 1

	  [Space(5)][Header(AlphaTest)]
	  [Space(5)]
	  [Toggle] _AlphaTest("AlphaTest", float) = 0
	  _Cutoff("AlphaClip", Range(0,1)) = 0.5
	  [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 1

	}

		SubShader
  {
 Tags
  {
  "RenderPipeline" = "UniversalPipeline"
  "RenderType" = "TransparentCutout"
  "Queue" = "AlphaTest"
   }

	LOD 100
		Cull[_Cull]

	  Pass
	  {
	  Name "Universal Forward"
	  Tags {"LightMode" = "UniversalForward"}

	  HLSLPROGRAM

	  #pragma prefer_hlslcc gles
	  #pragma exclude_renderers gles gles3 glcore
	  #pragma exclude_renderers d3d11_9x
	  #pragma target 3.0

	  //Normal mapping
	  #pragma shader_feature_local _NORMALMAP_ON	  
	  #pragma shader_feature_local_fragment _ALPHATEST_ON

	  // Universal Pipeline keywords
	  #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
	  #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
	  #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
	  #pragma multi_compile_fragment _ _SHADOWS_SOFT	  

	  // Unity defined keywords
	  #pragma multi_compile_fog

	  #pragma vertex vert
	  #pragma fragment frag

	  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"	 

	  CBUFFER_START(UnityPerMaterial)
	  float4 _BaseMap_ST;	  
	  half _Cutoff, _NormalInten;
	  float _FrensnelTerm, _Frensnel;
	  half4 _TintColor;
	  float _Aniso, _AnisoIntensity, _EnviIntensity, _Shift;	  
	  CBUFFER_END

	  Texture2D _BaseMap, _Flowmap;
	  Texture2D _Shiftmap;
	  SamplerState sampler_BaseMap;

      #if _NORMALMAP_ON
	  Texture2D _BumpMap;
      #endif

			  struct VertexInput
				{
				  float4 positionOS : POSITION;
				  float3 normalOS    : NORMAL;
				  float2 uv         : TEXCOORD0;
				  float4 tangentOS   : TANGENT;
				};

			  struct VertexOutput
				{
				  float4 positionCS  : SV_POSITION;
				  float3 normalWS    : NORMAL;

				  float2 uv           : TEXCOORD0;
				  float3 tangentWS    : TEXCOORD1;
				  float3 bitangentWS  : TEXCOORD2;
				  float3 viewDirWS    : TEXCOORD3;
				  half4  fogFactorAndVertexLight     : TEXCOORD4; // x: fogFactor, yzw: vertex light
				  float4 shadowCoord  : TEXCOORD5;
				  float3 positionWS   : TEXCOORD6;

				  };

			  //vertex stage

			  VertexOutput vert(VertexInput v)
					   {
						VertexOutput o = (VertexOutput)0;

						o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
						o.normalWS = TransformObjectToWorldNormal(v.normalOS.xyz);
						o.tangentWS = TransformObjectToWorldDir(v.tangentOS.xyz);
						o.bitangentWS = cross(o.normalWS, o.tangentWS) * v.tangentOS.w * unity_WorldTransformParams.w;
						o.positionWS = TransformObjectToWorld(v.positionOS.xyz);

						o.viewDirWS = _WorldSpaceCameraPos.xyz - TransformObjectToWorld(v.positionOS.xyz);
						o.uv = v.uv.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;

						//VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS.xyz);
						 half3 vertexLight = VertexLighting(o.positionWS, o.normalWS);
						 half fogFactor = ComputeFogFactor(o.positionCS.z);

					  #ifdef _MAIN_LIGHT_SHADOWS   
						 //o.shadowCoord = GetShadowCoord(vertexInput);
						   o.shadowCoord = float4(0,0,0,0);
						#endif
							o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);

						#ifdef _ADDITIONAL_LIGHTS_VERTEX
						
						uint lightsCount = GetAdditionalLightsCount();
							for (uint lightIndex = 0u; lightIndex < lightsCount; ++lightIndex)
							{
								Light light = GetAdditionalLight(lightIndex, o.positionWS);
								half3 lightColor = light.color * light.distanceAttenuation;
								vertexLight += LightingLambert(lightColor, light.direction, o.normalWS) * 0.05;
							}
						#endif

					   return o;
					   }

			  //Based on PCPlatform. R/5bit, G/6bit, B/5bit, A/8bit R>G, G>A if using mobile, modified *2.0 - 1.0.
			  //Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl

              #if _NORMALMAP_ON
					  inline half3 UnpackNormal(half4 packednormal)
					  {
						half3 normal;
						normal.xy = packednormal.wy * 2 - 1;
						normal.z = sqrt(1 - normal.x * normal.x - normal.y * normal.y);
						return normal;
					   }
              #endif
               
				  inline half3 TangentNormalToWorldNormal(float3 TangnetNormal, float3 T, float3  B, float3 N)
				   {
					 float3x3 TBN = float3x3(T, B, N);
					 TBN = transpose(TBN);
					 return mul(TBN, TangnetNormal);
				   }

				  inline float Hairspecular(float3 flow, float3 viewDir, float3 lightDir, float3 N, float aniso, float shift)
				  {
					  float3 hv = normalize(lightDir + viewDir);
					  flow = normalize(flow + shift * N);
					  float TdotH = dot(flow, hv);
					  float sinTH = sqrt(1.0 - TdotH * TdotH);
					  float specular = pow(sinTH, _Aniso)* smoothstep(-1.0, 0.0, TdotH);

					  return specular;
				  }

    	  //pixel stage

			half4 frag(VertexOutput i) : SV_Target
			{
			  //default function define     
			   float2 uv = i.uv;
			   float3 viewDir = normalize(i.viewDirWS);
			   i.shadowCoord = TransformWorldToShadowCoord(i.positionWS);
			   Light mainlight = GetMainLight(i.shadowCoord);
			   float3 lightDir = mainlight.direction;

			   //texture sampling
			   half4 diffuse = _BaseMap.Sample(sampler_BaseMap, i.uv) * _TintColor;
			   
			   half3 flowmap = _Flowmap.Sample(sampler_BaseMap, i.uv).rgb * 2.0 - 1.0;
			   half3 shift = _Shiftmap.Sample(sampler_BaseMap, i.uv).rgb - 0.5 + _Shift;
			   
			   #if _NORMALMAP_ON
				   half3 bump = UnpackNormal(_BumpMap.Sample(sampler_BaseMap, i.uv)) * half3(1, _NormalInten, 1);
				   float3 worldNormal = TangentNormalToWorldNormal(bump, i.tangentWS, i.bitangentWS, i.normalWS);
				   float NdotL = saturate(dot(worldNormal, lightDir));
               #else
				   float3 worldNormal = i.normalWS;
				   float NdotL = saturate(dot(i.normalWS, lightDir));
               #endif

			   //flowmapspec			 
			   
			   float3 flow = TangentNormalToWorldNormal(flowmap, i.tangentWS, i.bitangentWS, i.normalWS).xyz;
			   
			   // float3 hv = normalize(lightDir + viewDir);
			   //float NdotH = saturate(dot(hv, normalize(worldNormal)));

			   //flow = normalize(flow + shift * i.normalWS);
			   //float TdotH = dot(flow, hv);
			   //float sinTH = sqrt(1.0 - TdotH * TdotH);			   

			   //lighting			   			   
			   float specular = Hairspecular(flow, viewDir, lightDir.xyz, i.normalWS, _Aniso, shift) * _AnisoIntensity;
			   half3 specularColor = specular * diffuse.rgb * mainlight.color * NdotL * mainlight.shadowAttenuation;

			   //Environment Lighting
			   half3 reflectVector = reflect(-viewDir, worldNormal);
			   half4 encodedIrradiance = SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0, samplerunity_SpecCube0, reflectVector, 10);
			   
			   half3 irradiance = DecodeHDREnvironment(encodedIrradiance, unity_SpecCube0_HDR);

			   //BRDF function
			   half3 diffuseColor = diffuse.rgb * NdotL * mainlight.color ;

			   //fresnel term  
			   half fresnelTerm = saturate(dot(viewDir, worldNormal));
			   half3 rim = 1.0 - (pow(fresnelTerm, _Frensnel));

			   half3 ambient = SampleSH(worldNormal);


			   //Additional light
				#ifdef _ADDITIONAL_LIGHTS
				   uint pixelLightCount = GetAdditionalLightsCount();
				   for (uint lightIndex = 0u; lightIndex < pixelLightCount; ++lightIndex)
				   {
				   Light light = GetAdditionalLight(lightIndex, i.positionWS);

				   half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
				   diffuseColor += LightingLambert(attenuatedLightColor, light.direction, i.normalWS) * 0.05;
				// specularColor += LightingSpecular(attenuatedLightColor, light.direction, i.normalWS, viewDir, NdotH, 1);
				   specularColor += Hairspecular(flow, viewDir, light.direction, i.normalWS, _Aniso, shift) * attenuatedLightColor * 0.1;
				   }
				#endif

			   #ifdef _ADDITIONAL_LIGHTS_VERTEX
				  diffuseColor += i.fogFactorAndVertexLight.yzw;;
			   #endif

			   //combine light
			   diffuseColor += specularColor + (irradiance + rim * _FrensnelTerm) * ambient * _EnviIntensity;
			   
			   half4 color = half4(diffuseColor, diffuse.a);


			   #if _ALPHATEST_ON
			   clip(color.a - _Cutoff);
			   #endif

			   //apply fog
			   color.rgb = MixFog(color.rgb, i.fogFactorAndVertexLight.x);

			   return color;
			   }
			   ENDHLSL
			   }


			   Pass
			   {
			   Name "ShadowCaster"

			   Tags{"LightMode" = "ShadowCaster"}

			   Cull Back

			   HLSLPROGRAM

			   #pragma prefer_hlslcc gles
			   #pragma exclude_renderers d3d11_9x
			   #pragma target 2.0

			   #pragma vertex ShadowPassVertex
			   #pragma fragment ShadowPassFragment

			   #pragma shader_feature_local _ALPHATEST_ON
			   #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

			   #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

			  CBUFFER_START(UnityPerMaterial)
				float4 _BaseMap_ST;
			    half _Cutoff, _NormalInten;
			    float _FrensnelTerm, _Frensnel;
			    half4 _TintColor;
			    float _Aniso, _AnisoIntensity, _EnviIntensity, _Shift;
			  CBUFFER_END

			   Texture2D _BaseMap;
			   SamplerState sampler_BaseMap;

			   float3 _LightDirection;
			   float3 _LightPosition;			   

				struct VertexInput
				{
					float4 positionOS : POSITION;
					float4 normalOS : NORMAL;

					#if  _ALPHATEST_ON
					float2 uv     : TEXCOORD0;
					#endif
				};

				struct VertexOutput
				{
					float4 positionCS : SV_POSITION;

					#if  _ALPHATEST_ON
					float2 uv     : TEXCOORD0;
					#endif
				};

				float4 GetShadowPositionHClip(VertexInput i)
					{
						float3 positionWS = TransformObjectToWorld(i.positionOS.xyz);
						float3 normalWS = TransformObjectToWorldNormal(i.normalOS.xyz);

					#if _CASTING_PUNCTUAL_LIGHT_SHADOW
						float3 lightDirectionWS = normalize(_LightPosition - positionWS);
					#else
						float3 lightDirectionWS = _LightDirection;
					#endif

						float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

						return positionCS;
					}

				VertexOutput ShadowPassVertex(VertexInput v)
				{
				  VertexOutput o;

				 //float3 positionWS = TransformObjectToWorld(v.positionOS.xyz);
				 //float3 normalWS = TransformObjectToWorldNormal(v.normalOS.xyz);

				 //o.positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));
				 
				  o.positionCS = GetShadowPositionHClip(v);

				 #if _ALPHATEST_ON
				  o.uv = v.uv * _BaseMap_ST.xy + _BaseMap_ST.zw;
				 #endif 
				 return o;
				}

				half4 ShadowPassFragment(VertexOutput i) : SV_TARGET
				{
				#if _ALPHATEST_ON
				half alpha = _BaseMap.Sample(sampler_BaseMap, i.uv).a * _TintColor.a;
				clip(alpha - _Cutoff);
				#endif

				return 0;
				}

				ENDHLSL
			   }

			   Pass
			   {
			   Name "DepthOnly"
			   Tags{"LightMode" = "DepthOnly"}

			   Zwrite On
			   ColorMask 0

			   Cull Back

			   HLSLPROGRAM

			   #pragma prefer_hlslcc gles
			   #pragma exclude_renderers d3d11_9x
			   #pragma target 2.0

			   #pragma vertex vert
			   #pragma fragment frag

			   #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

				CBUFFER_START(UnityPerMaterial)
				float4 _BaseMap_ST;
				  half _Cutoff, _NormalInten;
				  float _FrensnelTerm, _Frensnel;
				  half4 _TintColor;
				  float _Aniso, _AnisoIntensity, _EnviIntensity, _Shift;
				  CBUFFER_END

			   struct VertexInput
			   {
			   float4 vertex : POSITION;
			   };

			   struct VertexOutput
			   {
			   float4 vertex : SV_POSITION;
			   };

			   VertexOutput vert(VertexInput v)
			   {
			   VertexOutput o;

			   o.vertex = TransformObjectToHClip(v.vertex.xyz);

			   return o;
			   }

			   half4 frag(VertexOutput IN) : SV_TARGET
			   {
			   return 0;
			   }
			   ENDHLSL
			   }
  }
}
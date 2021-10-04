void kajiyakay_float(float3 V, float3 T, float3 B, float3 N, float3 L, float shift, out float2 spec)
{
	float3 H = normalize(V + L);
	//using the binormal instead of Tangent since that goes root to tip
	//float3 binormalDir = cross(N, T);
	float3 binormalDir = B;

	//shift the tangent via spec map
	shift -= 0.5;
	float3 tangent1 = normalize(binormalDir + (0 + shift));
	float3 tangent2 = normalize(binormalDir + (0 + shift));

	//float3 tangent1 = normalize(B + (0 + shift) * N);
	//float3 tangent2 = normalize(B + (0 + shift) * N);
				
	//float2 spec = fixed2(0,0);
				
	//2 shifted specular terms, retuned as x,y components
	float dotTH = dot(tangent1, H);
	float sinTH = sqrt(1.0 - dotTH * dotTH);
	float dirAtten = smoothstep(-1.0, 0.0, dotTH);
	float2 specular = float2(0, 0);
	spec.x = dirAtten * pow(sinTH, _specularPower) * _specularIntensity;
	//spec.x = B.x;//
				
	dotTH = dot(tangent2, H);
	sinTH = sqrt(1.0 - dotTH * dotTH);
	dirAtten = smoothstep(-1.0, 0.0, dotTH);	
	spec.y = dirAtten * pow(sinTH, _specularPower2) * _specularIntensity2;
	//spec.y = B.y;
			
	spec = saturate(spec);
}

void strandSpecular_float(float3 T, float3 V, float3 L, float3 N, float exponent, float shift, out float spec)
{
	float3 H = normalize(L + V);
	T = normalize(T + shift * N);
	float dotTH = dot(T, H);
	float sinTH = sqrt(1.0 - dotTH * dotTH);
	float dirAtten = smoothstep(-1.0, 0.0, dotTH);

	spec = dirAtten * pow(sinTH, exponent);
}
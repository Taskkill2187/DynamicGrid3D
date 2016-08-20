//------------------------------------------------------
//--                                                  --
//--		   www.riemers.net                    --
//--   		    Basic shaders                     --
//--		Use/modify as you like                --
//--                                                  --
//------------------------------------------------------

struct VertexToPixel
{
	float4 Position   	  : POSITION;
	float4 Color		  : COLOR0;
	float LightingFactor  : TEXCOORD0;
	float3 Normal         : TEXCOORD1;
	float3 View           : TEXCOORD2;
	float2 TextureCoords  : TEXCOORD3;
};

struct PixelToFrame
{
	float4 Color : COLOR0;
};

//------- Constants --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float3 xLightDirection;
float xAmbient;
bool xEnableLighting;
bool xShowNormals;
float3 xCamPos;

//------- Technique: Colored --------

VertexToPixel ColoredVS(float4 inPos : POSITION, float3 inNormal : NORMAL, float4 inColor : COLOR)
{
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul(xView, xProjection);
	float4x4 preWorldViewProjection = mul(xWorld, preViewProjection);

	Output.Position = mul(inPos, preWorldViewProjection);
	Output.Color = inColor;

	float3 Normal = normalize(mul(normalize(inNormal), xWorld));
	Output.LightingFactor = 1;
	if (xEnableLighting)
		Output.LightingFactor = dot(Normal, -xLightDirection);
	Output.Normal = Normal;
	Output.View = normalize(float4(xCamPos, 1.0) - Output.Position);

	return Output;
}

PixelToFrame ColoredPS(VertexToPixel PSIn)
{
	PixelToFrame Output = (PixelToFrame)0;

	Output.Color = PSIn.Color;
	Output.Color.rgb *= saturate(PSIn.LightingFactor) + xAmbient;
	/*float4 reflect = float4(normalize(2 * PSIn.LightingFactor*PSIn.Normal - float4(xLightDirection, 1.0)), 1.0);*/
	float3 ref = reflect(xLightDirection, PSIn.Normal);
	float4 specular = pow(dot(ref, PSIn.View), 13);
	float lul = dot(PSIn.Normal, PSIn.View);

	if (lul > 0.5f) { lul = 1 - lul; }

	Output.Color += lul / 1.5f;
	/*Output.Color.a = 0.6f;*/

	return Output;
}

technique Colored
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 ColoredVS();
		PixelShader = compile ps_2_0 ColoredPS();
	}
}
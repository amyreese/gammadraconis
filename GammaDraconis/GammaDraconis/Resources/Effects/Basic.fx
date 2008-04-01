struct VS_OUTPUT
{
    float4 Position   	: POSITION;    
    float4 Color		: COLOR0;
    float LightingFactor: TEXCOORD0;
    float2 TextureCoords: TEXCOORD1;
};

struct PS_OUTPUT
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

//------- Texture Samplers --------

Texture xTexture;
sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

//------- Technique: Textured --------

VS_OUTPUT NormalVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float2 inTexCoords: TEXCOORD0)
{	
	VS_OUTPUT Output = (VS_OUTPUT)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection);	
	Output.TextureCoords = inTexCoords;
	
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
	Output.LightingFactor = 1;
	if (xEnableLighting)
		Output.LightingFactor = dot(Normal, -xLightDirection);
    
	return Output;    
}

PS_OUTPUT NormalPS(VS_OUTPUT PSIn) 
{
	PS_OUTPUT Output = (PS_OUTPUT)0;		
	
	Output.Color = tex2D(TextureSampler, PSIn.TextureCoords)*clamp(PSIn.LightingFactor + xAmbient,0,1);
	Output.Color = 0.0f;

	return Output;
}

technique Normal
{
	pass Pass0
    {   
    	VertexShader = compile vs_2_0 NormalVS();
        PixelShader  = compile ps_2_0 NormalPS();
    }
}
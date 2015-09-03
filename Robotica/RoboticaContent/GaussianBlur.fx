float4x4 World;
float4x4 View;
float4x4 Projection;

// TODO: add effect parameters here. 
sampler TextureSampler;

float blur = 0.0015;

struct PixelInput
{
	float2 TexCoord : TEXCOORD0;
};
float4 PixelShaderFunction(PixelInput input) : COLOR0
{

	float4 color = tex2D(TextureSampler,
	float2(input.TexCoord.x + blur, input.TexCoord.y + blur));
	color += tex2D(TextureSampler,
		float2(input.TexCoord.x - blur, input.TexCoord.y - blur));
	color += tex2D(TextureSampler,
		float2(input.TexCoord.x + blur, input.TexCoord.y - blur));
	color += tex2D(TextureSampler,
		float2(input.TexCoord.x - blur, input.TexCoord.y + blur));

	color = color / 4;
	return (color);
}

technique Default
{
	pass P0
	{
		// TODO: set renderstates here. 

		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
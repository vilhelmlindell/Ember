#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
 
sampler SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};
 
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR
{
    float4 color = SpriteTexture.Sample(SpriteTextureSampler, input.TextureCoordinates.xy);
    if (color.a == 0)
    {
        clip(-1);
    }
 
    return color;
}
 
technique SpriteDrawing
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}
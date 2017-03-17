// Per-vertex data from the vertex shader.
struct GeometryShaderInput
{
    min16float4 pos     : SV_POSITION;
    min16float3 color   : COLOR0;
    uint        instId  : TEXCOORD0;
};

// Per-vertex data passed to the rasterizer.
struct GeometryShaderOutput
{
    min16float4 pos     : SV_POSITION;
    min16float3 color   : COLOR0;
    uint        rtvId   : SV_RenderTargetArrayIndex;
};

#define CIRCLE_PRECISION 4

// This geometry shader is a pass-through that leaves the geometry unmodified 
// and sets the render target array index.
[maxvertexcount(CIRCLE_PRECISION * 3)]
void main(point GeometryShaderInput input[1], inout TriangleStream<GeometryShaderOutput> outStream)
{
    /*
    GeometryShaderOutput output;
    output.color = input[0].color;
    output.rtvId = input[0].instId;

    float PI = 3.141592;
    float t = 0;
    float cost = cos(t * PI * 2);
    float sint = sin(t * PI * 2);

    output.pos = input[0].pos;
    outStream.Append(output);

    output.pos = input[0].pos - min16float4(cost, sint, 0, 0);
    outStream.Append(output);

    output.pos = input[0].pos + min16float4(0, 1, 0, 0);
    outStream.Append(output);
    */

    GeometryShaderOutput output;
    output.color = input[0].color;
    output.rtvId = input[0].instId;

    min16float4 center = input[0].pos;

    float PI = 3.141592;
    float radius = 0.3f;

    [unroll(CIRCLE_PRECISION)]
    for ( int i = 0; i < CIRCLE_PRECISION; ++i )
    {
        float t1 = (float) i / CIRCLE_PRECISION;
        float cost1 = cos(t1 * PI * 2);
        float sint1 = sin(t1 * PI * 2);
        
        float t2 = (float) (i + 1) / CIRCLE_PRECISION;
        float cost2 = cos(t2 * PI * 2);
        float sint2 = sin(t2 * PI * 2);

        output.pos = center;
        outStream.Append(output);

        output.pos = center + radius * min16float4(cost1, sint1, 0, 0);
        outStream.Append(output);

        output.pos = center + radius * min16float4(cost2, sint2, 0, 0);
        outStream.Append(output);
    }
}

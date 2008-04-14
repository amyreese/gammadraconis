texture scene;
sampler sceneSampler = sampler_state
  {
  Texture = (scene);
  ADDRESSU = CLAMP;
  ADDRESSV = CLAMP;
  MAGFILTER = LINEAR;
  MINFILTER = LINEAR;
  MIPFILTER = LINEAR;
  };

float xcenter = 0.25; //center of the screen
float ycenter = 0.25;
float blurIntensity = 1.0f;
  
    float4 PixelShader(float2 texCoord : TEXCOORD0) : COLOR0
    {
    float4 color = tex2D(sceneSampler,texCoord);

    float2 coord = texCoord;
    for( int i=0; i<6; i++)
      {
        coord.x -= blurIntensity * (coord.x - xcenter) / 30;
        coord.y -= blurIntensity * (coord.y - ycenter) / 30;
        color +=tex2D(sceneSampler,coord);
      }
    color = color / 7;
        return color;
    }

    technique
    {
        pass
        {
            PixelShader = compile ps_2_0 PixelShader();
        }
    }
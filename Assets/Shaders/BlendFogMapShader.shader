Shader "CustomRenderTexture/BlendFogMap"
{
    Properties
    {
        _NextFog("NextFog", 2D) = "white" {}
     }

     SubShader
     {
        Lighting Off
        Blend One Zero

        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            sampler2D _NextFog;
            float _BlendSpeed;
            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                fixed prevFog = tex2D(_SelfTexture2D, IN.localTexcoord.xy);
                fixed nextFog = tex2D(_NextFog,  IN.localTexcoord.xy);
                fixed velocity = sign(nextFog - prevFog) * 0.01 * _BlendSpeed;
                fixed result = prevFog + velocity;                
                return clamp(result, 0, 1);
            }
        ENDCG
        }
    }
}
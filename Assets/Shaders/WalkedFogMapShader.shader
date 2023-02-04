Shader "CustomRenderTexture/WalkedFogMap"
{
    Properties
    {
        _NextFog("NextFog", 2D) = "black" {}
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
            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                fixed prevColor = tex2D(_SelfTexture2D, IN.localTexcoord.xy).r;
                fixed nextColor = tex2D(_NextFog, IN.localTexcoord.xy).r;
                return prevColor * nextColor;
            }
        ENDCG
        }
    }
}
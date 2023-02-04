Shader "Projector/ProjectorFogShader" {
    Properties {
		_FogTex ("Fog Texture", 2D) = "white" {}
        _FogWallkedTex("Fog Wallked Texture", 2D) = "white"{}
        _ColorFog ("Color Fog", Color) = (0,0,0,0)
        _ColorWallkedFog ("Color WallkedFog", Color) = (0,0,0,0)
    }
   
    Subshader {
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
   
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"
           
            struct v2f {
                float4 pos : SV_POSITION;
                float4 uvproj : TEXCOORD0;
            };
           
            float4x4 unity_Projector;
           
            v2f vert (appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);
                o.uvproj = mul (unity_Projector, v.vertex);
                return o;
            }
           
            sampler2D _FogTex;
            sampler2D _FogWallkedTex;
            fixed4 _ColorFog;
            fixed4 _ColorWallkedFog;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 projectedUV = i.uvproj.xy / i.uvproj.w;
                fixed NextFog = tex2D(_FogTex, projectedUV).r;
                fixed WalkedFog = 1 - tex2D(_FogWallkedTex, projectedUV).r;
                fixed4 NextFogColor = fixed4(_ColorFog.r, _ColorFog.g, _ColorFog.b, _ColorFog.a * NextFog) * (1 - WalkedFog);
                fixed4 WalkedFogColor = fixed4(_ColorWallkedFog.r, _ColorWallkedFog.g, _ColorWallkedFog.b, _ColorWallkedFog.a * WalkedFog) * NextFog * WalkedFog;
                return NextFogColor + WalkedFogColor;
            }
            ENDCG
        }
    }
}
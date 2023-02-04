using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOWProjector : MonoBehaviour
{
    public static event Action UpdateFOV = delegate{};
    [SerializeField] CustomRenderTexture outputFogMap;
    private Camera fogCamera;
    private Projector projector;
    private RenderTexture nextFogMap;

    [SerializeField] float blendSpeed = 3.33f;

    private static bool ReqUpdateFog;
    private static bool ReqUpdateFOV;

    void OnEnable()
    {
        fogCamera = GetComponentInChildren<Camera>();
        projector = GetComponent<Projector>();
        nextFogMap = new RenderTexture(fogCamera.targetTexture.width, fogCamera.targetTexture.height, 0);
        outputFogMap.material.SetTexture("_NextFog", nextFogMap);
        projector.material.SetTexture("_FogTex", outputFogMap);
    }
    public static void NeedUpdateFog() => ReqUpdateFog = true;
    public static void NeedUpdateFOVAndFog() { ReqUpdateFOV = true; ReqUpdateFog = true; }
    public void UpdateFog()
    {
        Graphics.Blit(fogCamera.targetTexture, nextFogMap);
        outputFogMap.material.SetFloat("_BlendSpeed", blendSpeed);
    }
    private void FixedUpdate()
    {
        if (ReqUpdateFOV)
        {
            UpdateFOV.Invoke();
            ReqUpdateFOV = false;
        }
        if (ReqUpdateFog)
        {
            
            UpdateFog();
            ReqUpdateFog = false;
        }
    }
    private void OnDestroy()
    {
        UpdateFOV = delegate { };
    }
}

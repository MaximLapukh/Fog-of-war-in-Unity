using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetWalkedMap : MonoBehaviour
{
    [SerializeField] CustomRenderTexture _walkedFogMap;
    void Start()
    {
        _walkedFogMap.Initialize();
    }
}

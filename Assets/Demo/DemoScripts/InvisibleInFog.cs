using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleInFog : MonoBehaviour
{
    private MeshRenderer _meshRender;
    void Start()
    {
        _meshRender = GetComponent<MeshRenderer>();
        FOWCallInvisbleEvent.VisibleObjectsEvent += ChangeState;
    }

    private void ChangeState(List<GameObject> obj)
    {
        if (obj.Contains(gameObject))
            _meshRender.enabled = true;
        else
            _meshRender.enabled = false;
    }

}

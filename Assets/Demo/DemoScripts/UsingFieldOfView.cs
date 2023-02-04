using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FOWFieldOfView))]
//using FieldOfView
public class UsingFieldOfView : MonoBehaviour
{
    private Vector3 _prevPosition;
    private float _distanceUpdate = 0.5f;

    private FOWFieldOfView _fieldOfView;
    void Start()
    {
        _fieldOfView = GetComponent<FOWFieldOfView>();
        _fieldOfView = GetComponent<FOWFieldOfView>();
        FOWProjector.UpdateFOV += ()=> { _fieldOfView.UpdateFieldOfView(); };
        FOWCallInvisbleEvent.RegisterFieldOfView(_fieldOfView);

    }
    void Update()
    {
        _fieldOfView.UpdateFieldOfView();
        if (Vector3.Distance(_prevPosition, transform.position) > _distanceUpdate)
        {
            _prevPosition = transform.position;
            FOWProjector.NeedUpdateFog();
        }
    }
}

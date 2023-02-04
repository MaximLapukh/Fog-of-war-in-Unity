using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform _target;
    private Vector3 _offset;
    private Vector3 _velocity;

    void Start()
    {
        _offset = transform.position - _target.position;
    }

    void Update()
    {
        
        transform.position = Vector3.SmoothDamp(transform.position, _target.position + _offset, ref _velocity, 0.5f);
    }
}

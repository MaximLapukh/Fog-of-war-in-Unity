using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOWCallInvisbleEvent : MonoBehaviour
{
    public static event Action<List<GameObject>> VisibleObjectsEvent = delegate { }; 
    private static List<FOWFieldOfView> FieldOfViews;
    private float _periodInvoke = .2f;
    private float _timer;
    private void Awake()
    {
        FieldOfViews = new List<FOWFieldOfView>();
    }
    void Update()
    {
        if (_timer <= 0)
        {
            _timer = _periodInvoke;
            List<GameObject> visibleObjects = new List<GameObject>();
            foreach (var item in FieldOfViews)
            {
                visibleObjects.AddRange(item.GetVisibleObject());
            }
            VisibleObjectsEvent.Invoke(visibleObjects);
        }
        else _timer -= Time.deltaTime;
    }
    public static void RegisterFieldOfView(FOWFieldOfView fieldOfView)
    {
        FieldOfViews.Add(fieldOfView);
    }
    private void OnDestroy()
    {
        VisibleObjectsEvent = delegate { };
    }
}

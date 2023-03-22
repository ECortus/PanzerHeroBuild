using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PinOnFollowObject : MonoBehaviour
{
    [SerializeField] private Transform pinObject;

    [Space]
    [SerializeField] private bool pinPosition = false;
    [SerializeField] private Vector3 Position;
    [SerializeField] private bool pinRotation = false;
    [SerializeField] private Vector3 EulerAngles;

    void Update()
    {
        if(pinPosition) transform.position = pinObject.position + Position;
        if(pinRotation) transform.eulerAngles = EulerAngles;
    }
}

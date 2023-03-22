using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PinOnFollowObject : MonoBehaviour
{
    [SerializeField] private Transform pinObject;

    [Space]
    [SerializeField] private Vector3 Position;
    [SerializeField] private Vector3 EulerAngles;

    void Update()
    {
        transform.position = pinObject.position + Position;
        transform.eulerAngles = EulerAngles;
    }
}

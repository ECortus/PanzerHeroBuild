using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotateUI : MonoBehaviour
{
    [SerializeField] private Vector3 rotate;

    void Update()
    {
        transform.eulerAngles = rotate;
    }
}

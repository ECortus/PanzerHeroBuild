using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWheelsForceAway : MonoBehaviour
{
    [SerializeField] private Rigidbody[] wheels;

    void OnEnable()
    {
        foreach(Rigidbody wheel in wheels)
        {
            wheel.transform.parent = null;
            wheel.AddForce(-wheel.transform.up * 1500f);
        }
    }

    void OnDisable()
    {
        foreach(Rigidbody wheel in wheels)
        {
            if(wheel != null) wheel.gameObject.SetActive(false);
        }
    }
}

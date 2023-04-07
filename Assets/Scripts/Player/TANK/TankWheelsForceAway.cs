using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TankWheelsForceAway : MonoBehaviour
{
    [SerializeField] private Rigidbody[] wheels;
    [SerializeField] private Collider[] cols;

    void OnEnable()
    {
        Enable();
    }

    async void Enable()
    {
        foreach(Rigidbody wheel in wheels)
        {
            /* wheel.transform.parent = null; */
            wheel.AddForce(-wheel.transform.up * 1500f);
        }

        await UniTask.Delay(5000);

        foreach(Rigidbody wheel in wheels)
        {
            wheel.isKinematic = true;
        }

        foreach(Collider col in cols)
        {
            col.enabled = false;
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

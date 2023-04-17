using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TankWheelsForceAway : MonoBehaviour
{
    [SerializeField] private Rigidbody[] wheels;
    [SerializeField] private Collider[] cols;

    [SerializeField] private float force = 1500f;

    void OnEnable()
    {
        Enable();
    }

    async void Enable()
    {
        foreach(Rigidbody wheel in wheels)
        {
            /* wheel.transform.parent = null; */
            wheel.isKinematic = false;

            Vector3 dir = -wheel.transform.up + new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f)
            );
            
            wheel.AddForce(dir * force);

            wheel.angularVelocity = new Vector3(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f)
            );
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

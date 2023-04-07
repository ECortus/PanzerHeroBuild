using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHeadForceAway : MonoBehaviour
{
    [SerializeField] private Rigidbody head;

    void OnEnable()
    {
        Enable();
    }

    void Enable()
    {
        Rigidbody rigid = head;
		if(rigid == null) return;
 
        rigid.useGravity = true;

        rigid.AddForce(1000f * Vector3.up);
        rigid.angularVelocity = new Vector3(
            Random.Range(-10f, 10f),
            Random.Range(-10f, 10f),
            Random.Range(-10f, 10f)
        );
    }

    void OnDisable()
    {
        gameObject.SetActive(false);
    }
}

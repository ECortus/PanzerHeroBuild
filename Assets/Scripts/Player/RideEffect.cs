using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _01, _02;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool Active = false;

    void Update()
    {
        if(rb.velocity.magnitude > 0.1f)
        {
            if(!Active) On();
        }
        else
        {
            if(Active) Off();
        }
    }

    public void On()
    {
        Active = true;
        _01.Play();
        _02.Play();
    }
    public void Off()
    {
        Active = false;
        _01.Stop();
        _02.Stop();
    }
}

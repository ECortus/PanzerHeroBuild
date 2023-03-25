using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] effects;

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
        foreach(ParticleSystem ps in effects)
        {
            if(ps != null) ps.Play();
        }
    }
    public void Off()
    {
        Active = false;
        foreach(ParticleSystem ps in effects)
        {
            if(ps != null) ps.Stop();
        }
    }
}

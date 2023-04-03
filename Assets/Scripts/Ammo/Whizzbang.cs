using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whizzbang : MonoBehaviour
{
    [SerializeField] private float speed;
    [HideInInspector] public float damage;
    
    private SphereCollider _sphere;
    private SphereCollider sphere
    {
        get
        {
            if(_sphere == null)
            {
                _sphere = GetComponent<SphereCollider>();
            }

            return _sphere;
        }
        set
        {
            _sphere = value;
        }
    }

    private TrailRenderer _trail;
    private TrailRenderer trial
    {
        get
        {
            if(_trail == null)
            {
                _trail = GetComponentInChildren<TrailRenderer>();
            }

            return _trail;
        }
        set
        {
            _trail = value;
        }
    }

    private Rigidbody _rb;
    public Rigidbody rb
    {
        get
        {
            if(_rb == null)
            {
                _rb = GetComponent<Rigidbody>();
            }

            return _rb;
        }
        set
        {
            _rb = value;
        }
    }

    [SerializeField] private GameObject destroyEffect;

    public bool Active => gameObject.activeSelf;

    public void On()
    {
        gameObject.SetActive(true);
        trial.Clear();
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }

    public void Reset(Vector3 pos, Vector3 rot)
    {
        transform.position = pos;
        transform.eulerAngles = rot;
    }

    public Vector3 center
    {
        get
        {
            return transform.TransformPoint(sphere.center);
            /* return transform.position; */
        }
    }

    void OnEnable()
    {
        rb.drag = 0f;
        rb.velocity = transform.forward * speed;
    }

    void Update()
    {
        if(Vector3.Distance(TankController.Instance.Transform.position, transform.position) > 200f) Off();
    }

    public void HitAboveSomething()
    {
        Off();
        if(destroyEffect != null) 
            ParticlePool.Instance.Insert(ParticleType.WhizzbangEffect, destroyEffect, transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whizzbang : MonoBehaviour
{
    [SerializeField] private float speed;
    private TrailRenderer trial;
    private Rigidbody rb;
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

    void OnEnable()
    {
        if(rb == null) rb = GetComponent<Rigidbody>();
        if(trial == null) trial = GetComponentInChildren<TrailRenderer>();

        rb.velocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision col)
    {
        GameObject go = col.gameObject;

        switch(go.tag)
        {
            case "Untagged":
                HitOnSomething();
                break;
            default:
                break;
        }
    }

    void HitOnSomething()
    {
        Off();
        if(destroyEffect != null) 
            ObjectPool.Instance.Insert(ObjectType.DestroyEffect, destroyEffect, transform.position, Vector3.zero);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [SerializeField] private Animation anim;

    private Transform target;

    public void SetTarget(Transform _target, bool animate = true)
    {
        target = _target;

        if(target != null)
        {
            this.enabled = true;
            if(animate)
            {
                anim.Play();
            }
            else
            {
                anim.Stop();
                hand.transform.localScale = Vector3.one;
            }

            transform.localPosition = target.localPosition;
        }
        else
        {
            this.enabled = false;
        }
    }

    void Start()
    {
        SetTarget(null);
    }

    void OnEnable()
    {
        hand.SetActive(true);
    }

    void OnDisable()
    {
        anim.Stop();
        hand.transform.localScale = Vector3.one;

        hand.SetActive(false);
    }
}

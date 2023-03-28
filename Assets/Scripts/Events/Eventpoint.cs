using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Eventpoint : MonoBehaviour
{
    [SerializeField] private UnityEvent Event;

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            Event.Invoke();
            gameObject.SetActive(false);
        }
    }
}

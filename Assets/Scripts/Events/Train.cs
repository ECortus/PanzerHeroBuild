using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Train : MonoBehaviour
{ 
    [SerializeField] private Transform spawnDot;

    [Space]
    [SerializeField] private Rigidbody rb;

    [Header("Parameters: ")]
    [SerializeField] private int delayToDisappear = 10000;
    [SerializeField] private float speed = 5f;

    public void On()
    {
        transform.position = spawnDot.position;
        gameObject.SetActive(true);
    }
    public void Off() => gameObject.SetActive(false);

    void Start()
    {
        Off();
    }

    public async void Move()
    {
        On();
        rb.velocity = transform.forward * speed;

        await UniTask.Delay(delayToDisappear);
        Off();
    }
}

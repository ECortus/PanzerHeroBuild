using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTouching : MonoBehaviour
{
    public bool isTouching = false;

    public Transform tank;
    [SerializeField] private Rigidbody rb;

    [Header("Boundes: ")]
    [Range(0f, 15f)]
    [SerializeField] private float topX;
    [Range(0f, 15f)]
    [SerializeField] private float bottomX;
    [SerializeField] private float duration;

    private float time;
    private float rotationX;

    public void Acceleration()
    {
        StopAllCoroutines();
        StartCoroutine(TouchingAcceleration());
    }

    public void Steering()
    {
        StopAllCoroutines();
        StartCoroutine(TouchingSteering());
    }

    IEnumerator TouchingAcceleration()
    {
        isTouching = true;
        time = 0f;

        rotationX = 0f;

        while(time < duration)
        {
            rb.MoveRotation(Quaternion.Euler(rotationX - topX * time, tank.localRotation.y, 0f));
			time += Time.deltaTime;

            if(PlayType.Get() != PlayState.Ride) break;

            yield return null;
        }

        time = duration;
        isTouching = false;

        yield return null;
    }

    IEnumerator TouchingSteering()
    {
        isTouching = true;
        time = 0f;

        rotationX = 0f;

        while(time < duration)
        {
            rb.MoveRotation(Quaternion.Euler(rotationX + bottomX * time, tank.localRotation.y, 0f));
			time += Time.deltaTime;

            if(PlayType.Get() != PlayState.Ride) break;

            yield return null;
        }

        time = duration;
        isTouching = false;

        yield return null;
    }
}

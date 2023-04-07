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
    [SerializeField] private float durationTouching, durationBack;

    private float time;
    private Quaternion rotation;
    private float angleX;

    public void Acceleration()
    {
        StopTouching();
        StartCoroutine(TouchingAcceleration());
    }

    public void Steering()
    {
        StopTouching();
        StartCoroutine(TouchingSteering());
    }

    public void StopTouching()
    {
        StopAllCoroutines();
        tank.localEulerAngles = new Vector3(0f, tank.localEulerAngles.y, 0f);
    }

    IEnumerator TouchingAcceleration()
    {
        isTouching = true;
        time = 0f;

        angleX = tank.localEulerAngles.x;

        while(time < durationTouching)
        {
            rotation = Quaternion.Euler(angleX - (topX - angleX) * time, tank.localEulerAngles.y, 0f);
            rb.MoveRotation(rotation);
			time += Time.deltaTime;

            if(PlayType.Get() != PlayState.Ride) break;

            yield return null;
        }

        time = durationTouching;
        isTouching = false;

        angleX = 360f - tank.localEulerAngles.x;
        time = 0f;

        while(time < durationBack)
        {
            rotation = Quaternion.Euler(angleX * time / durationBack - angleX, tank.localEulerAngles.y, 0f);
            rb.MoveRotation(rotation);
			time += Time.deltaTime;

            if(PlayType.Get() != PlayState.Ride) break;

            yield return null;
        }

        yield return null;
    }

    IEnumerator TouchingSteering()
    {
        isTouching = true;
        time = 0f;

        angleX = tank.localEulerAngles.x - 360f;
        angleX = Mathf.Abs(angleX) == 360f ? 0f : angleX;

        while(time < durationTouching / 1.2f)
        {
            rotation = Quaternion.Euler(angleX + (bottomX - angleX) * time, tank.localEulerAngles.y, 0f);
            rb.MoveRotation(rotation);
			time += Time.deltaTime;

            if(PlayType.Get() != PlayState.Ride) break;

            yield return null;
        }

        time = durationTouching;
        isTouching = false;

        angleX = tank.localEulerAngles.x;
        time = 0f;

        while(time < durationBack)
        {
            rotation = Quaternion.Euler(angleX - angleX * time / durationBack, tank.localEulerAngles.y, 0f);
            rb.MoveRotation(rotation);
			time += Time.deltaTime;

            if(PlayType.Get() != PlayState.Ride) break;

            yield return null;
        }

        yield return null;
    }
}

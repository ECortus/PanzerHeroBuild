using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTouching : MonoBehaviour
{
    public bool isTouching = false;

    [SerializeField] private Transform tank;
    /* [SerializeField] private Rigidbody rb; */

    [Header("Boundes: ")]
    [Range(0f, 15f)]
    [SerializeField] private float topX;
    [Range(0f, 15f)]
    [SerializeField] private float bottomX;
    [SerializeField] private float durationTouching, durationBack;

    private float time;
    private float angleX;

    public void Acceleration()
    {
        StopTouching(true);
        StartCoroutine(TouchingAcceleration());
    }

    public void Steering()
    {
        StopTouching();
        StartCoroutine(TouchingSteering());
    }

    public void StopTouching(bool defautlt = false)
    {
        StopAllCoroutines();
        if(defautlt) tank.localRotation = Quaternion.Euler(0f, tank.localEulerAngles.y, 0f);
    }

    IEnumerator TouchingAcceleration()
    {
        isTouching = true;
        time = 0f;

        angleX = tank.localEulerAngles.z;
        angleX = angleX < 0f ? 360f + angleX : angleX;

        while(time < durationTouching)
        {
            Set(-(angleX - (topX + angleX) * time));
			time += Time.deltaTime;

            yield return null;
        }

        time = durationTouching;
        isTouching = false;

        angleX = tank.localEulerAngles.z;
        time = 0f;

        while(time < durationBack)
        {
            Set(angleX - angleX * time / durationBack);
			time += Time.deltaTime;

            yield return null;
        }

        yield return null;
    }

    IEnumerator TouchingSteering()
    {
        isTouching = true;
        time = 0f;

        angleX = tank.localEulerAngles.z;
        angleX = angleX < 0f ? 360f + angleX : angleX;

        while(time < durationTouching / 1.2f)
        {
            Set(angleX - (bottomX + angleX) * time);
			time += Time.deltaTime;

            yield return null;
        }

        time = durationTouching;
        isTouching = false;

        angleX = tank.localEulerAngles.z - 360f;
        time = 0f;

        while(time < durationBack)
        {
            Set(angleX - angleX * time / durationBack);
			time += Time.deltaTime;

            yield return null;
        }

        Set(0f);

        yield return null;
    }

    void Set(float angle)
    {
        Quaternion rot = Quaternion.Euler(0f, tank.localEulerAngles.y, angle % 360f);
        tank.localRotation = rot;
        /* rb.MoveRotation(rot); */
    }
}

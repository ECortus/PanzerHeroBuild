using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavidJalbert;

public class EnemyTank : MonoBehaviour
{
	[SerializeField] private float rotateSpeed;
    [SerializeField] private float maxShootDistance = 35f;
    [SerializeField] private float distanceToDetect = 15f;

    [Space]
    [SerializeField] private Transform head;
    [SerializeField] private Transform gun;

    [Space]
	[SerializeField] private TinyCarController engine;
    [SerializeField] private EnemyTankShooting shooting;
    [SerializeField] private EnemyStats stats;

    public Vector3 center
	{
		get
		{
			return transform.position - transform.up * engine.colliderRadius * ((int)engine.colliderRadius / 1 + 1) / 10f;
		}
	}

    private Vector3 point
    {
        get
        {
            return TankController.Instance.center;
        }
    }

    [HideInInspector] public bool HaveDetectPlayer;

	void Start()
	{
        stats.Off();
	}

	private void FixedUpdate()
	{
        if(!HaveDetectPlayer)
        {
            if(stats.Active)
            {
                HaveDetectPlayer = true;
                return;
            }

            if(DistanceToPoint(point) < distanceToDetect)
            {
                HaveDetectPlayer = true;
                stats.On();
            }
            return;
        }

        if(!stats.Active || !GameManager.Instance.GetActive()) 
        {
            shooting.Off();
            engine.setMotor(0);
            return;
        }

		if(DistanceToPoint(point) < maxShootDistance)
		{
            shooting.On();
			engine.setMotor(0);
		}
        else
        {
            shooting.Off();
            engine.setMotor(2);

            Vector3 tv = -(point - transform.position).normalized;
            var rotation = Quaternion.LookRotation(tv);
            rotation = Quaternion.Euler(0f, rotation.eulerAngles.y + 180f, 0f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, rotateSpeed * Time.fixedDeltaTime);
        }

        RotateHead();
	}

    void RotateHead()
    {
        Vector3 tv = (point - transform.position).normalized;
        var rotation = Quaternion.LookRotation(tv);

        /* Debug.Log(rotation.eulerAngles); */

        Quaternion headRot = new Quaternion(0f, rotation.y - transform.rotation.y, 0f, rotation.w);
        Quaternion gunRot = new Quaternion(0f, 0f, -rotation.x, rotation.w);

        head.localRotation = Quaternion.Slerp(head.localRotation, headRot, rotateSpeed * 1.5f * Time.deltaTime);
        gun.localRotation = Quaternion.Slerp(gun.localRotation, gunRot, rotateSpeed * 4 * Time.deltaTime);
    }

	float DistanceToPoint(Vector3 point)
	{
		return Vector3.Distance(center, point);
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, maxShootDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, distanceToDetect);
    }
}

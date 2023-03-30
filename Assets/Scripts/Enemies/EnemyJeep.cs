using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavidJalbert;
using System.Linq;

public class EnemyJeep : MonoBehaviour
{
    [SerializeField] private bool LockMovement = false;

	[SerializeField] private float rotateSpeed;
    [SerializeField] private float maxShootDistance = 35f;
    [SerializeField] private float distanceToDetect = 15f;

    [Space]
	[SerializeField] private TinyCarController engine;
    [SerializeField] private EnemyStats stats;

    [Space]
    [SerializeField] private Transform patrolWayParent;
    private List<Transform> patrolWay = new List<Transform>();
    private int patrolIndex = 0;

    private Vector3 target;
    private Vector3 point
    {
        get
        {
            target = transform.position;

            if(HaveDetectPlayer || patrolWay.Count == 0) target = TankController.Instance.center;
            else target = patrolWay[patrolIndex].position;

            return target;
        }
    }

    public Vector3 center
	{
		get
		{
			return transform.position /* - transform.up * engine.colliderRadius * ((int)engine.colliderRadius / 1 + 1) / 10f */;
		}
	}

    [HideInInspector] public bool HaveDetectPlayer;

	void Start()
	{
        if(patrolWayParent != null)
        {
            patrolWay = patrolWayParent.GetComponentsInChildren<Transform>().ToList();
            patrolWay.RemoveAt(0);
        }

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

            if(DistanceToPoint(TankController.Instance.center) < distanceToDetect)
            {
                HaveDetectPlayer = true;
                stats.On();
                return;
            }

            if(!LockMovement) Patrol();
            return;
        }

        if(!stats.Active || !GameManager.Instance.GetActive()) 
        {
            engine.setMotor(0);

            engine.getBody().velocity = Vector3.zero;
            return;
        }

		if(DistanceToPoint(point) < maxShootDistance)
		{
			engine.setMotor(0);
		}
        else
        {
            if(!LockMovement) 
            {
                engine.setMotor(2);
            }
            else
            {
                engine.setMotor(0);
            }

            Vector3 tv = (point - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(tv);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, rotateSpeed * Time.fixedDeltaTime);
        }
	}

    void Patrol()
    {
        Debug.Log("patrol");
        if(patrolWay.Count == 0)
        {
            return;
        }

        if(DistanceToPoint(point) < 1.5f)
        {
            patrolIndex++;
            if(patrolIndex > patrolWay.Count - 1) patrolIndex = 0;
        }

        if(!LockMovement) 
        {
            engine.setMotor(2);

            Vector3 tv = (point - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(tv);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, rotateSpeed * Time.fixedDeltaTime);
        }
        else
        {
            engine.setMotor(0);
        }
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

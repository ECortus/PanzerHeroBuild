using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavidJalbert;
using System.Linq;

public class EnemyTank : MonoBehaviour
{
    [SerializeField] private bool LockMovement = false;

	[SerializeField] private float rotateSpeed;
    [SerializeField] private float maxShootDistance = 35f;
    [SerializeField] private float distanceToDetect = 15f;

    [Space]
    [SerializeField] private Transform head;
    [SerializeField] private Rigidbody brokenHead;
    [SerializeField] private Transform gun;

    [Space]
	[SerializeField] private TinyCarController engine;
    [SerializeField] private EnemyTankShooting shooting;
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
            shooting.Off();
            engine.setMotor(0);

            engine.getBody().velocity = Vector3.zero;
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
            if(!LockMovement) 
            {
                engine.setMotor(2);
            }
            else
            {
                engine.setMotor(0);
            }

            Rotate();
        }

        RotateHead();
	}

    void Patrol()
    {
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
            Drive();
        }
        else
        {
            engine.setMotor(0);
        }
    }

    void Drive()
    {
        engine.setMotor(2);
        Rotate();
    }

    void Rotate()
    {
        Vector3 tv = (point - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(tv);
        rotation = Quaternion.Euler(transform.localEulerAngles.x, rotation.eulerAngles.y, 0f);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, rotateSpeed * Time.fixedDeltaTime);
    }

    void RotateHead()
    {
        Vector3 tv = (point - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(tv);
        rotation = Quaternion.Euler(new Vector3(0f, rotation.eulerAngles.y, 0f) - transform.eulerAngles);

        /* Debug.Log(rotation.eulerAngles);
 */
        Quaternion headRot = new Quaternion(0f, rotation.y, 0f, rotation.w);
        Quaternion gunRot = new Quaternion(0f, 0f, -rotation.z, rotation.w);

        head.localRotation = Quaternion.Slerp(head.localRotation, headRot, rotateSpeed * 4f * Time.deltaTime);
        gun.localRotation = Quaternion.Slerp(gun.localRotation, gunRot, rotateSpeed * 4f * Time.deltaTime);
    }

	float DistanceToPoint(Vector3 point)
	{
		return Vector3.Distance(center, point);
	}

    public void SpawnEffectOnCenter(GameObject effect)
    {
        if(effect != null) ParticlePool.Instance.Insert(ParticleType.TankDestroyedEffect, effect, center);
    }

    void OnDrawGizmos()
    {
        /* Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, maxShootDistance); */

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, distanceToDetect);
    }
}

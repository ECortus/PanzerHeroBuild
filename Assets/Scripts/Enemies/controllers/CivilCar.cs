using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DavidJalbert;
public class CivilCar : MonoBehaviour
{
    [SerializeField] private bool LockMovement = false;

	[SerializeField] private float rotateSpeed;
    [SerializeField] private float stopDistance = 35f;
    [SerializeField] private float distanceToDetect = 15f;

    [Space]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Rigidbody bodyRB;
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
            target = patrolWay[patrolIndex].position;
            return target;
        }
    }

    public Vector3 center
	{
		get
		{
			return transform.position;
		}
	}

	void Start()
	{
        if(patrolWayParent != null)
        {
            patrolWay = patrolWayParent.GetComponentsInChildren<Transform>().ToList();
            patrolWay.RemoveAt(0);
        }

        stats.On();
	}

	private void FixedUpdate()
	{
        if(!LockMovement && stats.Active) 
        {
            Patrol();
            return;
        }
        
        if(!stats.Active || !GameManager.Instance.GetActive()) 
        {
            engine.setMotor(0);

            rb.velocity = Vector3.zero;
            return;
        }
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
            engine.setMotor(2);
            Rotate();
        }
        else
        {
            engine.setMotor(0);
        }
    }

    void Rotate()
    {
        Vector3 tv = (point - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(tv);
        rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, rotateSpeed * Time.fixedDeltaTime);
    }

    public void ForceBodyUp()
    {
        Rigidbody rigid = bodyRB;
        rigid.useGravity = true;

        rigid.AddForce(100000f * transform.up);
        rigid.angularVelocity = new Vector3(
            Random.Range(-10f, 10f),
            Random.Range(-10f, 10f),
            Random.Range(-10f, 10f)
        );
    }

    public void SpawnEffectOnCenter(GameObject effect)
    {
        if(effect != null) ParticlePool.Instance.Insert(ParticleType.TankDestroyedEffect, effect, center);
    }

	float DistanceToPoint(Vector3 point)
	{
		return Vector3.Distance(center, point);
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, stopDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, distanceToDetect);
    }
}

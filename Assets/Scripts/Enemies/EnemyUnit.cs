using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using System.Linq;

public class EnemyUnit : MonoBehaviour
{
    private readonly static int _Speed = Animator.StringToHash("Speed");
    private readonly static int _Shooting = Animator.StringToHash("Shooting");

    [SerializeField] private bool LockMovement = false;

    [Space]
    [SerializeField] private EnemyUnitShooting shooting;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private Collider collid;
    [SerializeField] private Rigidbody[] ragdollRBs;
    [SerializeField] private Collider[] ragdollCols;

    [Space]
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float maxShootDistance = 35f;
    [SerializeField] private float distanceToDetect = 8f;
    [SerializeField] private LayerMask wallMasks;
    [SerializeField] private LayerMask windowMask;

    [Space]
    public EnemyStats stats;

    public Vector3 center
	{
		get
		{
			return transform.position + new Vector3(0f, Agent.height * 1.1f, 0f);
		}
	}

    public Vector3 direction
    {
        get
        {
            return (point - center).normalized;
        }
    }

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

    [HideInInspector] public bool HaveDetectPlayer;

	void Start()
	{
        if(patrolWayParent != null)
        {
            patrolWay = patrolWayParent.GetComponentsInChildren<Transform>().ToList();
            patrolWay.RemoveAt(0);
        }

        if(LockMovement)
        {
            Agent.enabled = false;
            /* collid.enabled = false; */
        }

        MakeNoPhysical();
        shooting.Off();
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
            Agent.velocity = Vector3.zero;
            return;
        }

		if(HaveDetectPlayer)
		{
            if(DistanceToPoint(point) < maxShootDistance)
            {
                Agent.velocity = Vector3.zero;
                RotateHandle();

                Ray ray = new Ray(center, direction);
                
                bool wall = Physics.Raycast(ray, maxShootDistance, wallMasks);
                if(!wall)
                {
                    shooting.On();
                }
                else
                {
                    bool window = Physics.Raycast(ray, maxShootDistance, windowMask);
                    if(window)
                    {
                        shooting.On();
                    }
                    else
                    {
                        shooting.Off();
                    }
                }
            }
            else
            {
                shooting.Off();

                if(!LockMovement) 
                {
                    Agent.velocity = direction * speed;
                }
            }
		}

        if(LockMovement)
        {
            RotateHandle();
        }

        UpdateAnimator();
	}

    void Patrol()
    {
        if(patrolWay.Count == 0)
        {
            return;
        }

        if(DistanceToPoint(point) < 0.75f)
        {
            patrolIndex++;
            if(patrolIndex > patrolWay.Count - 1) patrolIndex = 0;
        }

        if(!LockMovement) 
        {
            Vector3 direction = (point - transform.position).normalized;
            Agent.velocity = direction * speed / 2f;
            UpdateAnimator();
        }
    }

    void UpdateAnimator()
    {
        animator.SetFloat(_Speed, Agent.velocity.magnitude);
        animator.SetBool(_Shooting, shooting.enabled);
    }

    void RotateHandle()
    {
        Vector3 tv = (point - transform.position).normalized;
        var rotation = Quaternion.LookRotation(tv);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.fixedDeltaTime);
    }

	float DistanceToPoint(Vector3 point)
	{
		return Vector3.Distance(center, point);
	}

    public void MakeNoPhysical()
    {
        foreach(Rigidbody rigid in ragdollRBs)
        {
            rigid.isKinematic = true;
        }

        animator.enabled = true;
    }

    public void MakePhysical(float force = 0f)
    {
        animator.enabled = false;

        Vector3 direction = Vector3.up + new Vector3(
            Random.Range(-1, 1),
            Random.Range(0.2f, 1),
            Random.Range(-1, 1)
        );

        foreach(Rigidbody rigid in ragdollRBs)
        {
            rigid.isKinematic = false;
            rigid.AddForce(direction * force);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(center, 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, maxShootDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, distanceToDetect);
    }
}

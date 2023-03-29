using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyUnit : MonoBehaviour
{
    private readonly static int _Speed = Animator.StringToHash("Speed");

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Rigidbody[] ragdollRBs;

    [Space]
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float maxShootDistance = 35f;
    [SerializeField] private float distanceToDetect = 8f;

    [Space]
    [SerializeField] private EnemyStats stats;

    public Vector3 center
	{
		get
		{
			return transform.position;
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
            return;
        }

		if(DistanceToPoint(point) < maxShootDistance)
		{

		}
        else
        {
            Vector3 direction = (point - transform.position).normalized;
            rb.velocity = direction * speed;
        }

        Vector3 tv = -(point - transform.position).normalized;
        var rotation = Quaternion.LookRotation(tv);
        rotation = Quaternion.Euler(0f, rotation.eulerAngles.y + 180f, 0f);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, rotateSpeed * Time.fixedDeltaTime);

        UpdateAnimator();
	}

    void UpdateAnimator()
    {
        animator.SetFloat(_Speed, rb.velocity.magnitude);
    }

    public void ForceOnDeath()
    {
        rb.AddForce(500 * Vector3.up);
    }

    public void ForceAway(float force, Vector3 direction)
    {
        rb.AddForce(force * direction);
    }

	float DistanceToPoint(Vector3 point)
	{
		return Vector3.Distance(center, point);
	}

    public void MakePhysical()
    {
        animator.enabled = false;
        foreach(Rigidbody rb in ragdollRBs)
        {
            
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, maxShootDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, distanceToDetect);
    }
}

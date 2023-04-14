using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavidJalbert;

public class TankController : MonoBehaviour
{
    public static TankController Instance { get; set; }

	public Transform Transform { get { return transform; } }
	public Vector3 center
	{
		get
		{
			return transform.position + transform.up * 1.5f;
		}
	}

	public Rigidbody rb;
	[SerializeField] private Transform head;
	[SerializeField] private float rotateSpeed;

	[SerializeField] private TinyCarController engine;
	public TankTouching Touching;

	private List<Vector3> points => LevelManager.Instance.ActualLevel.GetWayPoints();
	int pointIndex = 0;
	Vector3 point
	{
		get
		{
			Vector3 pos = pointIndex > points.Count - 1 ? points[points.Count - 1] : points[pointIndex];
			pos.y = transform.position.y;
			return pos;
		}
	}

	public void On() => this.enabled = true;
    public void Off() => this.enabled = false;

	private void OnEnable()
	{
		Instance = this;
		engine.enabled = true;

		rb.constraints = RigidbodyConstraints.None;
		rb.constraints = RigidbodyConstraints.FreezeRotationZ;
	}

	void OnDisable()
	{
		engine.setMotor(0);
		engine.enabled = false;

		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
		rb.constraints = RigidbodyConstraints.FreezeAll;
	}

	void Start()
	{
		
	}

	public void SpawnAtStart()
	{
		PlayerStats.Instance.ResetBody();
		Touching.StopTouching();

		pointIndex = 0;
		
		Vector3 pos = point;
		pos.y = transform.position.y;
		transform.position = pos;

		Vector3 tv = (points[pointIndex + 1] - point).normalized;
		var rotation = Quaternion.LookRotation(tv);
		rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
		transform.localRotation = rotation;

		head.localEulerAngles = Vector3.zero;

		pointIndex = 1;
	}

	private void FixedUpdate()
	{
		if(!PlayerStats.Instance.Active)
		{
			engine.setMotor(0);
			return;
		}

		if(pointIndex == points.Count)
		{
			PlayerStats.Instance.Off();
			LevelManager.Instance.ActualLevel.EndLevel();

			pointIndex = 0;
			return;
		}

		if(DistanceToPoint(point) < 4f)
		{
			pointIndex++;
		}

		if(TouchPad.Instance.HaveTouch)
		{
			engine.setMotor(2);

			if(Tutorial.Instance != null)
			{
				if(!Tutorial.Instance.Complete)
				{
					if(Tutorial.Instance.HOLD_isDone && !Tutorial.Instance.AIM_isDone)
					{
						Tutorial.Instance.SetState(TutorialState.NONE);
					}
				}
			}
		}
		else
        {
			engine.setMotor(0);
		}

		Vector3 tv = (point - transform.position).normalized;
		var rotation = Quaternion.LookRotation(tv);

		/* if(!touching.isTouching)  */rotation = Quaternion.Euler(transform.localEulerAngles.x, rotation.eulerAngles.y, 0f);
		/* else rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f); */

		/* rb.MoveRotation(Quaternion.Slerp(transform.localRotation, rotation, rotateSpeed * Time.fixedDeltaTime)); */
		transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, rotateSpeed * Time.fixedDeltaTime);

		Quaternion headRot = Quaternion.Euler(Vector3.zero);
        head.localRotation = Quaternion.Slerp(head.localRotation, headRot, rotateSpeed / 4f * Time.deltaTime);
	}

	public void SpawnEffectOnCenter(GameObject effect)
    {
        if(effect != null) ParticlePool.Instance.Insert(ParticleType.TankDestroyedEffect, effect, center);
    }

	public void ForceRigidbody(float force)
	{
		rb.AddForce(-transform.forward * force);
	}

	float DistanceToPoint(Vector3 point)
	{
		return Vector3.Distance(center, point);
	}

	/* void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
        Gizmos.DrawSphere(center, 0.5f);
	} */
}

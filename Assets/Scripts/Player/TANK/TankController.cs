using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavidJalbert;

public class TankController : MonoBehaviour
{
    public static TankController Instance { get; set; }

	public Transform Transform { get { return transform; } }

	public Rigidbody rb;
	[SerializeField] private float rotateSpeed;

	[SerializeField] private TinyCarController engine;

	private List<Vector3> points => LevelManager.Instance.ActualLevel.GetWayPoints();
	int pointIndex = 0;
	Vector3 point => points[pointIndex];

	private void OnEnable()
	{
		Instance = this;
	}

	void OnDisable()
	{
		rb.velocity = Vector3.zero;
	}

	void Start()
	{
		PlayerStats.Instance.On();
		SpawnAtStart();

		UI.Instance.Ride();
	}

	void SpawnAtStart()
	{
		pointIndex = 0;
		
		Vector3 pos = point;
		pos.y = transform.position.y;
		transform.position = pos;

		pointIndex = 1;
	}

	private void FixedUpdate()
	{
		if(!PlayerStats.Instance.Active)
		{
			engine.setMotor(0);
			return;
		}

		if(pointIndex + 1 == points.Count)
		{
			PlayerStats.Instance.Off();
		}

		if(DistanceToPoint(point) < 2.5f)
		{
			pointIndex++;
		}

		if(TouchPad.Instance.HaveTouch)
		{
			engine.setMotor(2);

			Vector3 tv = -(point - transform.position).normalized;
			var rotation = Quaternion.LookRotation(tv);
			rotation = Quaternion.Euler(0f, rotation.eulerAngles.y + 180f, 0f);

			/* rb.MoveRotation(Quaternion.Slerp(transform.localRotation, rotation, rotateSpeed * Time.fixedDeltaTime)); */
			transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, rotateSpeed * Time.fixedDeltaTime);
		}
		else
        {
			engine.setMotor(0);
		}
	}

	float DistanceToPoint(Vector3 point)
	{
		Vector3 center = transform.position - transform.up * engine.colliderRadius;
		return Vector3.Distance(center, point);
	}
}

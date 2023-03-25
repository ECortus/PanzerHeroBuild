using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavidJalbert;

public class TankController : MonoBehaviour
{
    public static TankController Instance { get; set; }

	public Transform Transform { get { return transform; } }

	[SerializeField] private Rigidbody rb;
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
		SpawnAtStart();

		UI.Instance.Ride();
	}

	void SpawnAtStart()
	{
		pointIndex = 0;
		transform.position = point;
		pointIndex = 1;
	}

	private void FixedUpdate()
	{
		if(pointIndex + 1 == points.Count)
		{
			engine.setMotor(0);
			return;
		}

		if(DistanceToPoint(point) < 1.5f)
		{
			pointIndex++;
		}

		if (TouchPad.HaveTouch)
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
		return Vector3.Distance(transform.position, point);
	}
}

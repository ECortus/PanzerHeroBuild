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
			return transform.position - transform.up * engine.colliderRadius * ((int)engine.colliderRadius / 1 + 1) / 4f;
		}
	}

	public Rigidbody rb;
	[SerializeField] private Transform head;
	[SerializeField] private float rotateSpeed;

	[SerializeField] private TinyCarController engine;
	[SerializeField] private TankTouching touching;

	private List<Vector3> points => LevelManager.Instance.ActualLevel.GetWayPoints();
	int pointIndex = 0;
	Vector3 point => pointIndex > points.Count - 1 ? points[points.Count - 1] : points[pointIndex];

	public void On() => this.enabled = true;
    public void Off() => this.enabled = false;

	private void OnEnable()
	{
		Instance = this;
	}

	void OnDisable()
	{
		engine.setMotor(0);
	}

	void Start()
	{
		
	}

	public void SpawnAtStart()
	{
		pointIndex = 0;
		
		Vector3 pos = point;
		pos.y = transform.position.y;
		transform.position = pos;

		Vector3 tv = (points[pointIndex + 1] - point).normalized;
		var rotation = Quaternion.LookRotation(tv);
		rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
		transform.localRotation = rotation;

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

		if(DistanceToPoint(point) < 3f)
		{
			pointIndex++;
		}

		if(TouchPad.Instance.HaveTouch)
		{
			engine.setMotor(2);
		}
		else
        {
			engine.setMotor(0);
		}

		Vector3 tv = (point - transform.position).normalized;
		var rotation = Quaternion.LookRotation(tv);

		if(!touching.isTouching) rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);

		/* rb.MoveRotation(Quaternion.Slerp(transform.localRotation, rotation, rotateSpeed * Time.fixedDeltaTime)); */
		transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, rotateSpeed * Time.fixedDeltaTime);

		Quaternion headRot = Quaternion.Euler(Vector3.zero);
        head.localRotation = Quaternion.Slerp(head.localRotation, headRot, rotateSpeed / 4f * Time.deltaTime);
	}

	public void ForceRigidbody(float force)
	{
		rb.AddForce(-transform.forward * force);
	}

	float DistanceToPoint(Vector3 point)
	{
		return Vector3.Distance(center, point);
	}
}

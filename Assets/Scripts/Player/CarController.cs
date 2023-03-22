using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public static CarController Instance { get; set; }

	[SerializeField] private CarEngine engine;
	[HideInInspector] public Vector3 move;
	public void SetMovePoint(Vector3 point)
	{
		move = (point - transform.position).normalized;
	}

	private void OnEnable()
	{
		Instance = this;
	}

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			move = Vector3.forward;
			engine.setMotor(2);
		}
		else
        {
			engine.setMotor(0);
		}
	}
}

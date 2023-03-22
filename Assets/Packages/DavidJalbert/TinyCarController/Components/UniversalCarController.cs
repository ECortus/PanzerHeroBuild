using DavidJalbert;
using UnityEngine;

public class UniversalCarController : MonoBehaviour
{
    private Rigidbody body;

    public TinyCarController carController;

    [HideInInspector] public Vector2 move;
    [HideInInspector] public bool takeControl;

    [SerializeField] private float rotationSpeed;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (takeControl)
        {
            body.velocity = Vector3.zero;
            carController.setMotor(0);
            return;
        }

        if (move != Vector2.zero)
        {
            carController.setMotor(2);
            
            Vector3 tv = new Vector3(move.x, 0, move.y);
            Quaternion rotation = Quaternion.LookRotation(tv);
            body.MoveRotation(Quaternion.Slerp(transform.localRotation, rotation, rotationSpeed));
        }
        else
        {
            carController.setMotor(0);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CarEngine : MonoBehaviour
{
    [SerializeField] private CarController carController;
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    [Header("Engine:")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;

    [Header("Wheels:")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    [Space]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    [Header("Ground:")]
    [SerializeField] private Transform ground;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody body;

    private int motor = 0;

    public bool onGround;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        onGround = GroundCheck();

        if(body.velocity.magnitude < 0.01f) body.velocity = Vector3.zero;

        if (onGround)
        {
            if(motor > 0)
            {
                isBreaking = false;

                Vector3 move = carController.move;
                verticalInput = move.z;
                horizontalInput = move.x;
            }
            else
            {
                isBreaking = true;
            }
        }
        else
        {
            verticalInput = 0f;
            horizontalInput = 0f;
            isBreaking = true;
        }

        HandleMotor();
        HandleSteering();
        /* UpdateWheels(); */
    }

    bool GroundCheck()
    {
        return Physics.CheckSphere(ground.position, 0.15f, groundMask);
    }

    private void HandleMotor() 
    {
        float value = verticalInput * motorForce;

        if(body.velocity.magnitude >= maxSpeed) value = 0f;

        frontLeftWheelCollider.motorTorque = value;
        frontRightWheelCollider.motorTorque = value;
        Breaking();
    }

    private void Breaking() 
    {
        currentbreakForce = isBreaking ? breakForce : 0f;

        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering() 
    {
        currentSteerAngle = 360f * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels() 
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) 
    {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    public void setMotor(int value)
    {
        motor = value;
    }
}

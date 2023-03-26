using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEngine : MonoBehaviour
{
    [SerializeField] private TankController main;

    [Header("Engine: ")]
    [SerializeField] private float maxSpeedForward;
    [SerializeField] private float accelerationForce;
    [SerializeField] private float brakeStrength;

    private float speed = 0;
    private Vector3 velocity;

    private int motor = 0;

    void FixedUpdate()
    {
        if(motor == 0 && speed > 0f)
        {
            speed -= brakeStrength * Time.deltaTime;
            if(speed < 0f)
            {
                speed = 0f;
            }
        }
        else if(motor > 0 && speed <= maxSpeedForward)
        {
            speed += accelerationForce * Time.fixedDeltaTime;
            if(speed > maxSpeedForward)
            {
                speed = maxSpeedForward;
            }
        }

        velocity = main.Transform.forward * speed;
        main.rb.velocity = velocity;
    }

    public void setMotor(int value)
    {
        motor = value;
    }
}

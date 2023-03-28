using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerCamera : MonoBehaviour
{
    public bool Active = false;

    [SerializeField] private Transform follow;
    public void SetFollow(Transform tf)
    {
        follow = tf;
    }

    [SerializeField] private float speedRide, rotateRide, speedAim, rotateAim = 2f;
    private float _speed
    {
        get
        {
            switch(PlayType.Get())
            {
                case PlayState.Aim:
                    return speedAim;
                default:
                    return speedRide;
            }
        }
    }
    private float _rotate
    {
        get
        {
            switch(PlayType.Get())
            {
                case PlayState.Aim:
                    return rotateAim;
                default:
                    return rotateRide;
            }
        }
    }

    private Vector3 destination;
    private Quaternion rotation;

    public void Activate() => Active = true; 
    public void Deactivate() => Active = false;

    void Start()
    {

    }

    void Update()
    {
        if(follow == null || !Active) return;

        destination = follow.position;
        rotation = follow.rotation;

        transform.position = Vector3.Lerp(
            transform.position, destination, _speed * Time.deltaTime
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation, rotation, _rotate * Time.deltaTime
        );
    }
}

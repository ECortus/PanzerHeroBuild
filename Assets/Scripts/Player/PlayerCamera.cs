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

    [SerializeField] private float speedMove = 2f;

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

#if UNITY_EDITOR
        transform.position = destination;
        transform.rotation = rotation;
#endif

        transform.position = Vector3.Lerp(
            transform.position, destination, speedMove * Time.deltaTime
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation, rotation, speedMove * Time.deltaTime
        );
    }
}

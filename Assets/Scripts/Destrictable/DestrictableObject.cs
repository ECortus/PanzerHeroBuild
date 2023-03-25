using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestrictableObject : MonoBehaviour
{
    private DestrictableAction destrictableScript;

    public Vector3 center
    {
        get
        {
            return transform.position;
        }
    }

    private Rigidbody _rb;
    public Rigidbody rb
    {
        get
        {
            if(_rb == null)
            {
                _rb = GetComponent<Rigidbody>();
                if(_rb == null)
                {
                    _rb = gameObject.AddComponent<Rigidbody>();
                }
            }

            return _rb;
        }
        set
        {
            _rb = value;
        }
    }

    private MeshCollider _col;
    public MeshCollider col
    {
        get
        {
            if(_col == null)
            {
                _col = GetComponent<MeshCollider>();
                if(_col == null)
                {
                    _col = gameObject.AddComponent<MeshCollider>();
                }
            }

            return _col;
        }
        set
        {
            _col = value;
        }
    }

    public void Setup(DestrictableAction act, PhysicMaterial mat)
    {
        destrictableScript = act;

        gameObject.tag = "Destrictable";
        gameObject.layer = 9;

        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        col.convex = true;
        col.isTrigger = false;
        col.material = mat;

        this.enabled = true;
    }

    void FixedUpdate()
    {
        CheckPhysicsDown();
    }

    public bool CheckPhysicsDown()
    {
        if(destrictableScript.DestrictableObjectsActive)
        {
            if(destrictableScript.HasToCheckFloor)
            {
                BoxCollider floorCollider = destrictableScript.floor;
                Transform floor = floorCollider.transform;

                Vector3 offset = Vector3.zero;

                offset = floor.up * (floor.lossyScale.y * floorCollider.size.y / 2f);
                Vector3 posTop = floor.position + offset;
                Vector3 posBottom = floor.position - offset;

                offset = floor.right * (floor.lossyScale.x * floorCollider.size.x / 2f);
                Vector3 posRight = floor.position + offset;
                Vector3 posLeft = floor.position - offset;

                float distanceToFloor = 2f;
                if(Vector3.Distance(transform.position, posTop) < distanceToFloor
                    || Vector3.Distance(transform.position, posBottom) < distanceToFloor
                    || Vector3.Distance(transform.position, posRight) < distanceToFloor
                    || Vector3.Distance(transform.position, posLeft) < distanceToFloor)
                {
                    return true;
                }
            }

            bool HasObject = false;
            float distanceToDownObject = 1.2f;

            if(Physics.Raycast(transform.position, Vector3.down, distanceToDownObject, destrictableScript.destrictableMask))
            {
                HasObject = true;
                return true;
            }

            if(!HasObject) 
            {
                Activate();
                ForceRigidbody(destrictableScript.destrictableForce / 1.5f, transform);

                return false;
            }
        }

        return true;
    }

    public void Activate()
    {
        gameObject.layer = 0;

        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;

        rb.WakeUp();

        col.isTrigger = false;

        destrictableScript.RemoveDestrictableObject(this);

        this.enabled = false;
    }

    public void ForceRigidbody(float force, Transform center)
    {
        Vector3 direction = center.forward;
            direction = new Vector3(
                direction.x + Random.Range(-1f, 1f),
                direction.y + Random.Range(0.1f, 1f),
                direction.z + Random.Range(-1f, 1f)
            );
        
        rb.AddForce(direction * force, ForceMode.Force);
        rb.angularVelocity = direction * force / 10f;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Whizzbang")
        {
            destrictableScript.GetDestroyed(col.gameObject.transform);
        }
    }
}

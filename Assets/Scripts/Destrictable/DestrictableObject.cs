using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

[ExecuteInEditMode]
public class DestrictableObject : MonoBehaviour
{
    private int Layer => (int)Mathf.Log(building.destrictableMask.value, 2);

    private DestrictableBuilding _building;
    public DestrictableBuilding building
    {
        get
        {   
            if(_building == null)
            {
                _building = GetComponentInParent<DestrictableBuilding>();
            }
            return _building;
        }
    }

    public Vector3 center
    {
        get
        {
            Bounds bounds = col.sharedMesh.bounds;
            Vector3 point = transform.TransformPoint(bounds.center);
            return point;
            /* return transform.position; */
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

    private bool HasObject = false;

    public void Setup()
    {
        gameObject.tag = "Destrictable";
        gameObject.layer = Layer;

        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        col.convex = true;
        col.isTrigger = false;
        col.material = building.destrictableMaterial;

        this.enabled = true;
    }

    void Update()
    {
        /* if(building.CheckPhysicsAlready) CheckPhysicsDown(); */
    }

    /* public async void ProofPhysics()
    {
        CheckPhysicsDown();
        await UniTask.Delay(10);
        CheckPhysicsDown();
    } */

    public void CheckPhysicsDown()
    {
        float distanceToDownObject = 2f;

        RaycastHit hit;

        Debug.DrawLine(center, center + Vector3.down * distanceToDownObject, Color.red);

        if(Physics.Raycast(center, Vector3.down, out hit, distanceToDownObject, Layer))
        {
            Debug.Log($"{hit.transform.name}: distance - {Vector3.Distance(center, hit.point)}, layer - {hit.transform.gameObject.layer}");
            HasObject = true;
            return;
        }

        if(!HasObject) 
        {
            /* Trigger(); */
            /* ForceRigidbody(building.destrictableForce / 1.5f, transform.forward); */

            return;
        }
    }

    public void Trigger()
    {
        /* gameObject.layer = 0; */

        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;

        rb.WakeUp();

        col.isTrigger = false;

        building.RemoveDestrictableObject(this);

        this.enabled = false;
    }

    public void ForceRigidbody(float force, Vector3 direction)
    {
        direction = new Vector3(
            direction.x + Random.Range(-1f, 1f),
            direction.y + Random.Range(0.1f, 1f),
            direction.z + Random.Range(-1f, 1f)
        );
        
        rb.AddForce(direction * force, ForceMode.Force);
        rb.angularVelocity = direction * force / 10f;
    }
}

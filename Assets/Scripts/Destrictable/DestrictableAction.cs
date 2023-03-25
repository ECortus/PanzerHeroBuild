using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DestrictableAction : MonoBehaviour
{
    public bool HasToCheckFloor = false;
    public BoxCollider floor;

    [SerializeField] private List<DestrictableObject> destrictableObjects;
    [SerializeField] private GameObject wholeParent, destrictableParent;
    public bool DestrictableObjectsActive => destrictableParent.activeSelf;
    [SerializeField] private Collider col;

    [Space]
    [SerializeField] private PhysicMaterial destrictableMaterial;

    [Space]
    public float destrictableForce;
    public float destrictableRadius;
    public LayerMask destrictableMask;

    void Start()
    {
        RefreshComponentsToParts();
    }

    [ContextMenu("Refresh Components To Parts")]
    void RefreshComponentsToParts()
    {
        destrictableObjects.Clear();

        foreach(Transform trans in destrictableParent.transform)
        {
            GameObject go = trans.gameObject;
            DestrictableObject destObj = go.GetComponent<DestrictableObject>();

            if(destObj == null)
            {
                destObj = go.AddComponent<DestrictableObject>();
            }
            destObj.Setup(this, destrictableMaterial);
            destrictableObjects.Add(destObj);
        }
    }

    public void GetDestroyed(Transform hitter)
    {
        wholeParent.SetActive(false);
        destrictableParent.SetActive(true);

        List<DestrictableObject> obj = GetObjectsOnRadius(hitter.position);

        foreach(DestrictableObject destObj in obj)
        {
            destObj.Activate();
            destObj.ForceRigidbody(destrictableForce, hitter);
        }

        /* for(int i = 0; i < destrictableObjects.Count; )
        {
            if(destrictableObjects[i].CheckPhysicsDown())
            {
                i++;
            }
        } */

        Whizzbang wb = null;
        if(obj.Count > 0 && hitter.TryGetComponent<Whizzbang>(out wb)) wb.HitAboveSomething();

        if(destrictableObjects.Count == 0) col.enabled = false;
    }

    public void RemoveDestrictableObject(DestrictableObject obj)
    {
        destrictableObjects.Remove(obj);
    }

    List<DestrictableObject> GetObjectsOnRadius(Vector3 center)
    {
        List<DestrictableObject> list = new List<DestrictableObject>();

        foreach(DestrictableObject destObj in destrictableObjects)
        {
            if(list.Contains(destObj)) continue;

            float radius = destrictableRadius;
            float distance = Vector3.Distance(center, destObj.center);

            if(distance <= radius) list.Add(destObj);

            ///additional objects upper in cyclinder
            /* foreach(DestrictableObject obj in destrictableObjects)
            {
                if(list.Contains(obj)) continue;

                Vector3 point1 = obj.transform.position;
                Vector3 point2 = center;
                point1.y = 0f;
                point2.y = 0f;

                if(Vector3.Distance(point1, center) <= radius * 1.2f)
                {
                    point1.y = obj.transform.position.y;
                    if(point1.y >= point2.y)
                    {
                        list.Add(obj);
                    }
                }
            } */
        }

        return list;
    }
}

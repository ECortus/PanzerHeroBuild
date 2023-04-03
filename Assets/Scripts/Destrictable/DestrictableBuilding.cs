using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DestrictableBuilding : MonoBehaviour
{
    [SerializeField] private DestrictableObject floor;

    public bool CheckPhysicsAlready = false;
    [SerializeField] private Transform destrictableParent;

    [Space]
    public List<DestrictableObject> destrictableObjects;

    [Header("Parameters: ")]
    public PhysicMaterial destrictableMaterial;
    public LayerMask destrictableMask, enemyMask;
    public float destrictableForce;
    public float destrictableRadius;

    void Start()
    {
        RefreshComponentsToParts();
    }

    [ContextMenu("Refresh")]
    void RefreshComponentsToParts()
    {
        destrictableObjects.Clear();

        foreach(Transform trans in destrictableParent)
        {
            GameObject go = trans.gameObject;
            DestrictableObject destObj = go.GetComponent<DestrictableObject>();

            if(destObj == null)
            {
                destObj = go.AddComponent<DestrictableObject>();
            }
            destObj.Setup();
            destrictableObjects.Add(destObj);
        }
    }

    public void RemoveDestrictableObject(DestrictableObject obj)
    {
        destrictableObjects.Remove(obj);
    }

    public void GetDestroyedByTank(Transform tank)
    {
        TriggerDestrictableObjects(destrictableObjects, tank.forward);
    }

    public void GetDestroyedByWhizzbang(Whizzbang whizzbang)
    {
        if(whizzbang.damage < 1f) return;

        TriggerDestrictableObjects(GetObjectsOnRadius(whizzbang.center), whizzbang.transform.forward);

        Collider[] cols = Physics.OverlapSphere(whizzbang.center, destrictableRadius * 1.75f, enemyMask);

        if(cols.Length > 0)
        {
            EnemyUnit unit;
            foreach(Collider col in cols)
            {
                unit = col.GetComponentInParent<EnemyUnit>();
                if(unit != null)
                {
                    unit.stats.GetHit(999f);
                }
            }
        }
    }

    void TriggerDestrictableObjects(List<DestrictableObject> objects, Vector3 forward)
    {
        List<DestrictableObject> list = objects;

        int count = list.Count;
        for(int i = 0; i < count; i++)
        {
            if(list.Count == 0) break;
            if(i >= list.Count) i = list.Count - 1;

            DestrictableObject destObj = list[i];

            if(floor != null)
                if(destObj == floor) continue;

            Vector3 direction = forward + new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(0.2f, 1f),
                Random.Range(-1f, 1f)
            );

            destObj.Trigger();
            destObj.ForceRigidbody(destrictableForce, direction);
        }
    }

    List<DestrictableObject> GetObjectsOnRadius(Vector3 center)
    {
        List<DestrictableObject> list = new List<DestrictableObject>();

        foreach(DestrictableObject destObj in destrictableObjects)
        {
            if(list.Contains(destObj)) continue;

            float distance = Vector3.Distance(center, destObj.center);
            float radius = destrictableRadius;

            if(distance <= radius) list.Add(destObj);

            ///additional objects upper in cyclinder
            foreach(DestrictableObject obj in destrictableObjects)
            {
                if(list.Contains(obj)) continue;

                Vector3 point1 = obj.transform.position;
                Vector3 point2 = center;
                point1.y = 0f;
                point2.y = 0f;

                if(Vector3.Distance(point1, center) <= radius * 1.2f)
                {
                    point1.y = obj.transform.position.y;
                    point2.y = center.y;
                    if(point1.y >= point2.y)
                    {
                        list.Add(obj);
                    }
                }
            }
        }

        return list;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhizzbangCollision : MonoBehaviour
{
    [SerializeField] private Whizzbang main;
    [SerializeField] private LayerMask unitMask;

    void OnCollisionEnter(Collision col)
    {
        GameObject go = col.gameObject;
        Debug.Log($"{gameObject.name} collision: {go.tag}");

        EnemyStats stats;
        DestrictableBuilding building;

        switch(go.tag)
        {
            case "Untagged":
                main.HitAboveSomething();
                break;
            case "Player":
                PlayerStats.Instance.GetHit(main.damage);
                main.HitAboveSomething();
                break;
            case "EnemyTank":
                stats = col.gameObject.GetComponent<EnemyStats>();
                if(stats != null) stats.GetHit(main.damage);
                main.HitAboveSomething();
                break;
            case "EnemyUnit":
                stats = col.gameObject.GetComponentInParent<EnemyStats>();
                if(stats != null && !stats.isDead) stats.GetHit(main.damage);
                main.HitAboveSomething();
                break;
            case "Building":
                /* building = col.gameObject.GetComponent<DestrictableBuilding>();
                building.GetDestroyedByWhizzbang(main); */
                main.HitAboveSomething();
                break;
            case "Destrictable":
                building = col.gameObject.GetComponent<DestrictableObject>().building;
                if(col.gameObject.GetComponentInParent<House>() != null)
                {
                    building.GetDestroyedByWhizzbang(main, true);
                }
                else
                {
                    building.GetDestroyedByWhizzbang(main);
                }
                main.HitAboveSomething();
                break;
            case "Ground":
                main.HitAboveSomething();
                break;
            default:
                break;
        }

        CheckEnemyUnitNear();
    }

    void CheckEnemyUnitNear()
    {
        Collider[] cols = Physics.OverlapSphere(main.center, 1.75f, unitMask);
        if(cols.Length > 0)
        {
            EnemyStats unit;
            foreach(Collider coll in cols)
            {
                unit = coll.GetComponent<EnemyStats>();
                if(unit != null && !unit.isDead)
                {
                    unit.GetHit(999f);
                }
            }
        }
    }
}

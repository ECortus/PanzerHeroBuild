using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhizzbangCollision : MonoBehaviour
{
    [SerializeField] private Whizzbang main;

    void OnCollisionEnter(Collision col)
    {
        GameObject go = col.gameObject;

        main.rb.velocity = Vector3.zero;
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
                col.gameObject.GetComponent<EnemyStats>().GetHit(main.damage);
                main.HitAboveSomething();
                break;
            case "EnemyUnit":
                col.gameObject.GetComponentInParent<EnemyStats>().GetHit(main.damage);
                main.HitAboveSomething();
                break;
            case "Building":
                main.HitAboveSomething();
                break;
            case "Destrictable":
                DestrictableBuilding building = col.gameObject.GetComponent<DestrictableObject>().building;
                building.GetDestroyedByWhizzbang(main);
                main.HitAboveSomething();
                break;
            case "Ground":
                main.HitAboveSomething();
                break;
            default:
                break;
        }

        Debug.Log($"{gameObject.name} collision: {go.tag}");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCollision : MonoBehaviour
{
    [SerializeField] private TankController main;

    void OnCollisionEnter(Collision col)
    {
        GameObject go = col.gameObject;

        switch(go.tag)
        {
            case "Untagged":
                break;
            case "Building":
                break;
            case "EnemyUnit":
                col.gameObject.GetComponent<EnemyStats>().GetHit(999f);
                break;
            case "Destrictable":
                break;
            case "Ground":
                break;
            default:
                break;
        }
    }
}

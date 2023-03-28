using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnRoad : DestrictableBuilding
{
    void OnTriggerEnter(Collider col)
    {
        GameObject go = col.gameObject;

        switch(go.tag)
        {
            case "Player":
                GetDestroyedByTank(col.transform);
                break;
            case "EnemyTank":
                GetDestroyedByTank(col.transform);
                break;
            default:
                break;
        }
    }
}

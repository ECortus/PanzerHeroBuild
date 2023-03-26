using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierOnRoad : DestrictableBuilding
{
    void OnTriggerEnter(Collider col)
    {
        GameObject go = col.gameObject;

        switch(go.tag)
        {
            case "Player":
                GetDestroyedByPlayer(col.GetComponent<TankController>());
                break;
            default:
                break;
        }
    }
}

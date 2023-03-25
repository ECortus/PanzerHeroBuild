using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierOnRoad : DestrictableAction
{
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            GetDestroyed(col.transform);
        }
        else if(col.tag == "Whizzbang")
        {
            GetDestroyed(col.transform);
        }
    }
}
